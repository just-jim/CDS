using CDS.Domain.AssetAggregate.ValueObjects;
using CDS.Domain.Common.Models;
using CDS.Domain.ContentDistributionAggregate.ValueObjects;

namespace CDS.Domain.AssetAggregate.Entities;

public sealed class AssetContentDistribution : Entity<AssetContentDistributionId> {
    public ContentDistributionId ContentDistributionId { get; private set; }
    public string FileUrl { get; private set; }

    AssetContentDistribution(ContentDistributionId contentDistributionId, string fileUrl)
        : base(AssetContentDistributionId.CreateUnique()) {
        ContentDistributionId = contentDistributionId;
        FileUrl = fileUrl;
    }

    public static AssetContentDistribution Create(ContentDistributionId contentDistributionId, string fileUrl) {
        return new AssetContentDistribution(contentDistributionId, fileUrl);
    }
}