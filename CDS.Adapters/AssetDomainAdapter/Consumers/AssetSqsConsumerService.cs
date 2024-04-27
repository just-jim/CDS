using Amazon.SQS;
using CDS.Adapters.AssetDomainAdapter.Models.Sqs;
using CDS.Adapters.Interfaces;
using CDS.Adapters.Poller;
using Microsoft.Extensions.Logging;

namespace CDS.Adapters.AssetDomainAdapter.Consumers;

public class AssetSqsConsumerService(ILogger<AssetSqsConsumerService> logger, IAmazonSQS sqs) : ISqsConsumerService {
    const string QueueName = "assets";

    public Type GetMessageObjectType() {
        return typeof(AssetDomainAsset);
    }

    public async Task StartAsync(CancellationToken ct) {
        var poller = new SqsPoller(logger, sqs);
        await poller.Polling(QueueName, GetMessageObjectType(), HandleMessage, ct);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    public void HandleMessage(IMessage? message) {
        var asset = (AssetDomainAsset?)message;
        logger.LogInformation($"AssetSqsConsumerService received the asset {asset?.AssetId}");
    }
}