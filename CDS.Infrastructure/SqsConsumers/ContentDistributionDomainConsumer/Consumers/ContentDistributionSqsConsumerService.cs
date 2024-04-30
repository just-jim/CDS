using Amazon.SQS;
using CDS.Application.ContentDistributions.Commands.CreateContentDistribution;
using CDS.Infrastructure.SqsConsumers.ContentDistributionDomainConsumer.Models.Sqs;
using CDS.Infrastructure.SqsConsumers.Interfaces;
using CDS.Infrastructure.SqsConsumers.Poller;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CDS.Infrastructure.SqsConsumers.ContentDistributionDomainConsumer.Consumers;

public class ContentDistributionSqsConsumerService(ILogger<ContentDistributionSqsConsumerService> logger, IAmazonSQS sqs, IConfiguration configuration, ISender mediator) : ISqsConsumerService {
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

    public async void HandleMessage(IMessage message) {
        var contentDistribution = (ContentDistributionDomainContentDistribution)message;
        logger.LogInformation($"ContentDistributionSqsConsumerService received the content distribution for the date {contentDistribution.DistributionDate}");

        var distributionDate = DateOnly.Parse(contentDistribution.DistributionDate);
        var command = new CreateContentDistributionCommand(distributionDate,contentDistribution.DistributionChannel,contentDistribution.DistributionMethod);
        await mediator.Send(command);
    }
}