using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.Models;
using CDS.Domain.OrderAggregate.ValueObjects;

namespace CDS.Domain.AssetAggregate.Entities;

public sealed class AssetOrder : Entity<AssetOrderId> {
    public OrderId OrderId { get; private set; }
    public int Quantity { get; private set; }

    AssetOrder(OrderId orderId, int quantity)
        : base(AssetOrderId.CreateUnique()) {
        OrderId = orderId;
        Quantity = quantity;
    }

    public static AssetOrder Create(OrderId orderId, int quantity) {
        return new AssetOrder(orderId, quantity);
    }
}