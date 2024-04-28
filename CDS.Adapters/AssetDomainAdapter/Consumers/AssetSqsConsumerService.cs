using Amazon.SQS;
using CDS.Adapters.AssetDomainAdapter.Models.Sqs;
using CDS.Adapters.Interfaces;
using CDS.Adapters.Poller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CDS.Adapters.AssetDomainAdapter.Consumers;

public class AssetSqsConsumerService(ILogger<AssetSqsConsumerService> logger, IAmazonSQS sqs, IConfiguration configuration) : ISqsConsumerService {
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

    public void HandleMessage(IMessage? message) {
        var asset = (AssetDomainAsset?)message;
        logger.LogInformation($"AssetSqsConsumerService received the asset {asset?.AssetId}");
    }
}