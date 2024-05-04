using CDS.Domain.Common.Models.identities;

namespace CDS.Domain.OrderAggregate.ValueObjects;

public sealed class AssetOrderId : EntityId<Guid> {
    AssetOrderId(Guid value) : base(value) {
    }

    public static AssetOrderId Create(Guid value) {
        return new AssetOrderId(value);
    }

    public static AssetOrderId CreateUnique() {
        return new AssetOrderId(Guid.NewGuid());
    }
}