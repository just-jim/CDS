using CDS.Domain.Common.Models.identities;

namespace CDS.Domain.AssetAggregate.ValueObjects;

public sealed class AssetContentDistributionId : EntityId<Guid> {
    AssetContentDistributionId(Guid value) : base(value) {
    }

    public static AssetContentDistributionId CreateUnique() {
        return new AssetContentDistributionId(Guid.NewGuid());
    }
}