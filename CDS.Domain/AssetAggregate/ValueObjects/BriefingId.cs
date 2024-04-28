using CDS.Domain.Common.Models.identities;

namespace CDS.Domain.AssetAggregate.ValueObjects;

public sealed class BriefingId : EntityId<Guid> {
    BriefingId(Guid value) : base(value) {
    }

    public static BriefingId CreateUnique() {
        return new BriefingId(Guid.NewGuid());
    }
}