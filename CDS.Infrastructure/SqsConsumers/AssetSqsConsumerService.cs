using Amazon.SQS;
using CDS.Application.Assets.Commands.CreateAsset;
using CDS.Application.Common.Interfaces.Consumers;
using CDS.Application.Common.Interfaces.Models;
using CDS.Application.Common.Models;
using CDS.Infrastructure.SqsConsumers.Poller;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CDS.Infrastructure.SqsConsumers;

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
        
        logger.LogInformation($"consumed Asset with id '{asset.AssetId}'");
        var command = new CreateAssetCommand(asset.AssetId,asset.Name,asset.Description,asset.FileFormat,asset.FileSize,asset.Path);
        await mediator.Send(command);
    }
}