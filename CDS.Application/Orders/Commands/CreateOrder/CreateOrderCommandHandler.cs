using CDS.Application.Common.Interfaces.Database;
using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.DomainErrors;
using CDS.Domain.OrderAggregate;
using CDS.Domain.OrderAggregate.Entities;
using CDS.Domain.OrderAggregate.ValueObjects;
using ErrorOr;
using MediatR;

namespace CDS.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository) : IRequestHandler<CreateOrderCommand, ErrorOr<Order>> {

    public async Task<ErrorOr<Order>> Handle(CreateOrderCommand request, CancellationToken ct) {
        if (await orderRepository.ExistsAsync(OrderId.Create(request.OrderNumber))) {
            return Errors.OrderError.AlreadyExists;
        }

        var order = Order.Create(
            request.OrderNumber,
            request.CustomerName,
            request.OrderDate,
            request.TotalAssets,
            request.AssetOrders.ConvertAll(assetOrder => AssetOrder.Create(
                    AssetId.Create(assetOrder.AssetId),
                    assetOrder.Quantity
                )
            )
        );

        await orderRepository.AddAsync(order);

        return order;
    }
}