using CDS.Domain.Common.Models.identities;

namespace CDS.Domain.AssetAggregate.ValueObjects;

public sealed class AssetOrderId : EntityId<Guid> {
    AssetOrderId(Guid value) : base(value) {
    }

    public static AssetOrderId CreateUnique() {
        return new AssetOrderId(Guid.NewGuid());
    }
}