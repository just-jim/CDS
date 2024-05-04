using CDS.Domain.Common.Models.identities;

namespace CDS.Domain.ContentDistributionAggregate.ValueObjects;

public sealed class AssetContentDistributionId : EntityId<Guid> {
    AssetContentDistributionId(Guid value) : base(value) {
    }

    public static AssetContentDistributionId Create(Guid value) {
        return new AssetContentDistributionId(value);
    }

    public static AssetContentDistributionId CreateUnique() {
        return new AssetContentDistributionId(Guid.NewGuid());
    }
}