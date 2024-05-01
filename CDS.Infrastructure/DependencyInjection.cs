using Amazon.Runtime;
using Amazon.SQS;
using CDS.Application.Common.Interfaces.Database;
using CDS.Infrastructure.Database;
using CDS.Infrastructure.Database.Interceptors;
using CDS.Infrastructure.Database.Repositories;
using CDS.Infrastructure.SqsConsumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace CDS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration,
        IHostBuilder host
        ) {
        services
            .AddDatabase(configuration)
            .AddSqsClient(configuration)
            .AddSqsConsumers()
            .AddLogger(host);
        
        return services;
    }

    static IServiceCollection AddSqsClient(
        this IServiceCollection services,
        IConfiguration configuration
    ) {
        var amazonSqsClient = new AmazonSQSClient(
            new BasicAWSCredentials("ignore", "ignore"),
            new AmazonSQSConfig { ServiceURL = configuration.GetConnectionString("LocalStack") }
        );
        services.AddSingleton<IAmazonSQS>(_ => amazonSqsClient);
        
        return services;
    }
    
    static IServiceCollection AddSqsConsumers(
        this IServiceCollection services
    ) {
        // To allow polling within IHostedServices we need to allow concurrent running of services
        services.Configure<HostOptions>(x => {
            x.ServicesStartConcurrently = true;
            x.ServicesStopConcurrently = false;
        });
        
        // Add the sqs consumers to consume messages from the external domains
        services.AddHostedService<AssetSqsConsumerService>();
        services.AddHostedService<OrderSqsConsumerService>();
        services.AddHostedService<ContentDistributionSqsConsumerService>();
        
        return services;
    }
    
    static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    ) {
        services.AddDbContext<CdsDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("Postgres")),
            ServiceLifetime.Singleton,
            ServiceLifetime.Singleton
        );
        
        services.AddSingleton<PublishDomainEventsInterceptor>();
        services.AddSingleton<IAssetRepository, AssetRepository>();
        services.AddSingleton<IOrderRepository, OrderRepository>();
        services.AddSingleton<IContentDistributionRepository, ContentDistributionRepository>();

        return services;
    }

    static IServiceCollection AddLogger(
        this IServiceCollection services,
        IHostBuilder host
    ) {
        host.UseSerilog((context, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Filter.ByExcluding(c => c.Exception is TaskCanceledException)
            .Enrich.FromLogContext()
            .WriteTo.Console()
        );
        
        return services;
    }
}