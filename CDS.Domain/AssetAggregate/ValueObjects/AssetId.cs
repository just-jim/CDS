using CDS.Domain.Common.Models.identities;

namespace CDS.Domain.AssetAggregate.ValueObjects;

public sealed class AssetId : AggregateRootId<string> {
    AssetId(string value) : base(value) {
    }

    public static AssetId Create(string value) {
        return new AssetId(value);
    }
}