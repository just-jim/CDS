using Amazon.SQS;
using CDS.Adapters.Interfaces;
using CDS.Adapters.OrderDomainAdapter.Models.Sqs;
using CDS.Adapters.Poller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CDS.Adapters.OrderDomainAdapter.Consumers;

public class OrderSqsConsumerService(ILogger<OrderSqsConsumerService> logger, IAmazonSQS sqs, IConfiguration configuration) : ISqsConsumerService {
    public Type GetMessageObjectType() {
        return typeof(OrderDomainOrder);
    }

    public async Task StartAsync(CancellationToken ct) {
        var poller = new SqsPoller(logger, sqs);
        await poller.Polling(configuration["OrdersQueueName"]!, GetMessageObjectType(), HandleMessage, ct);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    public void HandleMessage(IMessage? message) {
        var order = (OrderDomainOrder?)message;
        logger.LogInformation($"OrderSqsConsumerService received the order {order?.OrderNumber}");
    }
}