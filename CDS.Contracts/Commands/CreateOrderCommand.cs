using CDS.Domain.OrderAggregate;
using ErrorOr;
using MediatR;

namespace CDS.Contracts.Commands;

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