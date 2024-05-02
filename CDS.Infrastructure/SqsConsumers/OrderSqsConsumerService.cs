﻿using Amazon.SQS;
using CDS.Application.Common.Interfaces.Consumers;
using CDS.Application.Common.Models;
using CDS.Application.Orders.Commands.CreateOrder;
using CDS.Domain.OrderAggregate;
using CDS.Infrastructure.SqsConsumers.Poller;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CDS.Infrastructure.SqsConsumers;

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
        logger.LogInformation($"consumed Order with number '{order.OrderNumber}'");
        
        var orderDate = DateOnly.Parse(order.OrderDate);
        List<AssetOrderCommand> assetOrderCommands = 
            order.Assets.ConvertAll(assetOrder => 
                new AssetOrderCommand(assetOrder.AssetId,assetOrder.Quantity)
            );
        var command = new CreateOrderCommand(order.OrderNumber,order.CustomerName,orderDate,order.TotalAssets,assetOrderCommands);
        ErrorOr<Order> createdOrder = await mediator.Send(command);
        if (createdOrder.IsError) {
            logger.LogInformation($"Order {order.OrderNumber} failed to be created. Reason: {createdOrder.FirstError.Description}");
        }
    }
}