using Amazon.SQS;
using CDS.Infrastructure.SqsConsumers.AssetDomainConsumer.Models.Sqs;
using CDS.Infrastructure.SqsConsumers.Interfaces;
using CDS.Infrastructure.SqsConsumers.Poller;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CDS.Application.Assets.Commands.CreateAsset;

namespace CDS.Infrastructure.SqsConsumers.AssetDomainConsumer.Consumers;

public class AssetSqsConsumerService(ILogger<AssetSqsConsumerService> logger, IAmazonSQS sqs, IConfiguration configuration, ISender mediator) : ISqsConsumerService {
    public Type GetMessageObjectType() {
        return typeof(AssetDomainAsset);
    }

    public async Task StartAsync(CancellationToken ct) {
        var poller = new SqsPoller(logger, sqs);
        await poller.Polling(configuration["AssetsQueueName"]!, GetMessageObjectType(), HandleMessage, ct);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    public async void HandleMessage(IMessage message) {
        var asset = (AssetDomainAsset)message;
        
        logger.LogInformation($"AssetSqsConsumerService received the asset {asset.AssetId}");
        var command = new CreateAssetCommand(asset.AssetId,asset.Name,asset.Description,asset.FileFormat,asset.FileSize,asset.Path);
        await mediator.Send(command);
    }
}