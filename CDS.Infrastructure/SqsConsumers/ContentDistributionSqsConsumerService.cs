using Amazon.SQS;
using CDS.Contracts.Commands;
using CDS.Contracts.Interfaces.Consumers;
using CDS.Contracts.Models.ExternalDomains;
using CDS.Infrastructure.SqsConsumers.Poller;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CDS.Infrastructure.SqsConsumers;

public class ContentDistributionSqsConsumerService(IServiceProvider serviceProvider) : ISqsConsumerService {

    readonly ILogger<AssetSqsConsumerService> _logger = serviceProvider.GetRequiredService<ILogger<AssetSqsConsumerService>>();
    readonly IAmazonSQS _sqs = serviceProvider.GetRequiredService<IAmazonSQS>();
    readonly IConfiguration _configuration = serviceProvider.GetRequiredService<IConfiguration>();

    public Type GetMessageObjectType() {
        return typeof(ContentDistributionDomainContentDistribution);
    }

    public async Task StartAsync(CancellationToken ct) {
        var poller = new SqsPoller(_logger, _sqs);
        await poller.Polling(_configuration["ContentDistributionsQueueName"]!, GetMessageObjectType(), HandleMessage, ct);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    public async void HandleMessage(IMessage message) {
        var contentDistribution = (ContentDistributionDomainContentDistribution)message;
        _logger.LogInformation($"consumed Content distribution for the date '{contentDistribution.DistributionDate}'");

        var distributionDate = DateOnly.Parse(contentDistribution.DistributionDate);
        List<AssetContentDistributionCommand> assetContentDistributionCommands =
            contentDistribution.Assets.ConvertAll(assetContentDistribution =>
                new AssetContentDistributionCommand(assetContentDistribution.AssetId, assetContentDistribution.FileURL)
            );
        var command = new CreateContentDistributionCommand(distributionDate, contentDistribution.DistributionChannel, contentDistribution.DistributionMethod, assetContentDistributionCommands);
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(command);
    }
}