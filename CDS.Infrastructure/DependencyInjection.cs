using Amazon.Runtime;
using Amazon.SQS;
using CDS.Contracts.Interfaces.Cache;
using CDS.Contracts.Interfaces.Clients;
using CDS.Contracts.Interfaces.Database;
using CDS.Infrastructure.Cache;
using CDS.Infrastructure.Clients;
using CDS.Infrastructure.Database;
using CDS.Infrastructure.Database.Interceptors;
using CDS.Infrastructure.Database.Repositories;
using CDS.Infrastructure.SqsConsumers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using StackExchange.Redis;

namespace CDS.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration,
        IHostBuilder host
    ) {
        services
            .AddDatabase(configuration)
            .AddSqsClient(configuration)
            .AddSqsConsumers()
            .AddCaching(configuration)
            .AddQueryServices()
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
        services.AddHostedService(provider => new AssetSqsConsumerService(provider.CreateScope().ServiceProvider));
        services.AddHostedService(provider => new OrderSqsConsumerService(provider.CreateScope().ServiceProvider));
        services.AddHostedService(provider => new ContentDistributionSqsConsumerService(provider.CreateScope().ServiceProvider));

        return services;
    }

    static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration
    ) {
        services.AddDbContext<CdsDbContext>(
            options => options.UseNpgsql(configuration.GetConnectionString("Postgres"))
        );

        services.AddScoped<PublishDomainEventsInterceptor>();
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IContentDistributionRepository, ContentDistributionRepository>();

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

    static IServiceCollection AddQueryServices(
        this IServiceCollection services
    ) {
        services.AddHttpClient<IQueryService, BriefingQueryService>();

        return services;
    }

    static IServiceCollection AddCaching(
        this IServiceCollection services,
        IConfiguration configuration
    ) {
        var redisOptions = new RedisCacheOptions {
            ConfigurationOptions = new ConfigurationOptions()
        };
        redisOptions.ConfigurationOptions.EndPoints.Add(configuration.GetConnectionString("Redis")!);
        IOptions<RedisCacheOptions> opts = Options.Create(redisOptions);

        services.AddSingleton<IDistributedCache>(_ => new RedisCache(opts));
        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}