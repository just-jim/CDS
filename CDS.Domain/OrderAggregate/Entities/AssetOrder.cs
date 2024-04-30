using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.Models;
using CDS.Domain.OrderAggregate.ValueObjects;

namespace CDS.Domain.OrderAggregate.Entities;

public sealed class AssetOrder : Entity<AssetOrderId> {
    public AssetId AssetId { get; private set; }
    public int Quantity { get; private set; }

    AssetOrder(AssetId assetId, int quantity)
        : base(AssetOrderId.CreateUnique()) {
        AssetId = assetId;
        Quantity = quantity;
    }

    public static AssetOrder Create(AssetId assetId, int quantity) {
        return new AssetOrder(assetId, quantity);
    }
}