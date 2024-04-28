using CDS.Domain.Common.Models.identities;

namespace CDS.Domain.ContentDistributionAggregate.ValueObjects;

public sealed class ContentDistributionId : AggregateRootId<Guid> {
    ContentDistributionId(Guid value) : base(value) {
    }

    public static ContentDistributionId CreateUnique() {
        return new ContentDistributionId(Guid.NewGuid());
    }
}