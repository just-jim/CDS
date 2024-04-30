using CDS.Domain.OrderAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    string OrderNumber,
    string CustomerName,
    DateOnly OrderDate,
    int TotalAssets,
    List<AssetOrderCommand> AssetOrders
) : IRequest<ErrorOr<Order>>;

public record AssetOrderCommand(
    string AssetId,
    int Quantity
);
