using Amazon.SQS;
using CDS.Application.Orders.Commands.CreateOrder;
using CDS.Domain.OrderAggregate;
using CDS.Infrastructure.SqsConsumers.Interfaces;
using CDS.Infrastructure.SqsConsumers.OrderDomainConsumer.Models.Sqs;
using CDS.Infrastructure.SqsConsumers.Poller;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CDS.Infrastructure.SqsConsumers.OrderDomainConsumer.Consumers;

public class OrderSqsConsumerService(ILogger<OrderSqsConsumerService> logger, IAmazonSQS sqs, IConfiguration configuration, ISender mediator) : ISqsConsumerService {
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

    public async void HandleMessage(IMessage message) {
        var order = (OrderDomainOrder)message;
        logger.LogInformation($"OrderSqsConsumerService received the order {order.OrderNumber}");
        
        var orderDate = DateOnly.Parse(order.OrderDate);
        var command = new CreateOrderCommand(order.OrderNumber,order.CustomerName,orderDate,order.TotalAssets);
        
        ErrorOr<Order> createdOrder = await mediator.Send(command);
        if (createdOrder.IsError) {
            logger.LogInformation($"Order {order.OrderNumber} failed to be created. Reason: {createdOrder.FirstError.Description}");
        }
    }
}