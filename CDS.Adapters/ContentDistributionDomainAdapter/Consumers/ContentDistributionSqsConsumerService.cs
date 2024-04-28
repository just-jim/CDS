using Amazon.SQS;
using CDS.Adapters.ContentDistributionDomainAdapter.Models.Sqs;
using CDS.Adapters.Interfaces;
using CDS.Adapters.Poller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CDS.Adapters.ContentDistributionDomainAdapter.Consumers;

public class ContentDistributionSqsConsumerService(ILogger<ContentDistributionSqsConsumerService> logger, IAmazonSQS sqs, IConfiguration configuration) : ISqsConsumerService {
    public Type GetMessageObjectType() {
        return typeof(ContentDistributionDomainContentDistribution);
    }

    public async Task StartAsync(CancellationToken ct) {
        var poller = new SqsPoller(logger, sqs);
        await poller.Polling(configuration["ContentDistributionsQueueName"]!, GetMessageObjectType(), HandleMessage, ct);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    public void HandleMessage(IMessage? message) {
        var contentDistribution = (ContentDistributionDomainContentDistribution?)message;
        logger.LogInformation($"ContentDistributionSqsConsumerService received the content distribution for the date {contentDistribution?.DistributionDate}");
    }
}