using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.OrderAggregate;
using CDS.Domain.OrderAggregate.ValueObjects;
using MediatR;

namespace CDS.Application.Common.Interfaces.Database;

public interface IOrderRepository {
    Task AddAsync(Order order);
    Task<bool> ExistsAsync(OrderId orderId);
    Task<List<Order>> FindOrdersByAssetId(AssetId assetId);
    Task<Unit> ResetAsync();
}