using Amazon.SQS;
using CDS.Application.Common.Interfaces.Consumers;
using CDS.Application.Common.Models;
using CDS.Application.Orders.Commands.CreateOrder;
using CDS.Domain.OrderAggregate;
using CDS.Infrastructure.SqsConsumers.Poller;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CDS.Infrastructure.SqsConsumers;

public class OrderSqsConsumerService(IServiceProvider serviceProvider) : ISqsConsumerService {
    
    readonly ILogger<AssetSqsConsumerService> _logger = serviceProvider.GetRequiredService<ILogger<AssetSqsConsumerService>>();
    readonly IAmazonSQS _sqs = serviceProvider.GetRequiredService<IAmazonSQS>();
    readonly IConfiguration _configuration = serviceProvider.GetRequiredService<IConfiguration>();
    
    public Type GetMessageObjectType() {
        return typeof(OrderDomainOrder);
    }

    public async Task StartAsync(CancellationToken ct) {
        var poller = new SqsPoller(_logger, _sqs);
        await poller.Polling(_configuration["OrdersQueueName"]!, GetMessageObjectType(), HandleMessage, ct);
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        return Task.CompletedTask;
    }

    public async void HandleMessage(IMessage message) {
        var order = (OrderDomainOrder)message;
        _logger.LogInformation($"consumed Order with number '{order.OrderNumber}'");
        
        var orderDate = DateOnly.Parse(order.OrderDate);
        List<AssetOrderCommand> assetOrderCommands = 
            order.Assets.ConvertAll(assetOrder => 
                new AssetOrderCommand(assetOrder.AssetId,assetOrder.Quantity)
            );
        var command = new CreateOrderCommand(order.OrderNumber,order.CustomerName,orderDate,order.TotalAssets,assetOrderCommands);
        
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        ErrorOr<Order> createdOrder = await mediator.Send(command);
        if (createdOrder.IsError) {
            _logger.LogInformation($"Order {order.OrderNumber} failed to be created. Reason: {createdOrder.FirstError.Description}");
        }
    }
}