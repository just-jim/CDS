using Amazon.Runtime;
using Amazon.SQS;
using CDS.Infrastructure.SqsConsumers.AssetDomainConsumer.Consumers;
using CDS.Infrastructure.SqsConsumers.ContentDistributionDomainConsumer.Consumers;
using CDS.Infrastructure.SqsConsumers.OrderDomainConsumer.Consumers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CDS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddSqsClient(configuration)
            .AddSqsConsumers();
        
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
}