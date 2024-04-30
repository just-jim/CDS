using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.Models;
using CDS.Domain.ContentDistributionAggregate.ValueObjects;

namespace CDS.Domain.ContentDistributionAggregate.Entities;

public sealed class AssetContentDistribution : Entity<AssetContentDistributionId> {
    public AssetId AssetId { get; private set; }
    public string FileUrl { get; private set; }

    AssetContentDistribution(AssetId assetId, string fileUrl)
        : base(AssetContentDistributionId.CreateUnique()) {
        AssetId = assetId;
        FileUrl = fileUrl;
    }

    public static AssetContentDistribution Create(AssetId assetId, string fileUrl) {
        return new AssetContentDistribution(assetId, fileUrl);
    }
}