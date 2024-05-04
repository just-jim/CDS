using CDS.Domain.Common.Models;
using CDS.Domain.ContentDistributionAggregate.Entities;
using CDS.Domain.ContentDistributionAggregate.Events;
using CDS.Domain.ContentDistributionAggregate.ValueObjects;

namespace CDS.Domain.ContentDistributionAggregate;

public class ContentDistribution : AggregateRoot<ContentDistributionId, Guid> {
    readonly List<AssetContentDistribution> _assetContentDistributions = [];

    public DateOnly DistributionDate { get; private set; }
    public string DistributionChannel { get; private set; }
    public string DistributionMethod { get; private set; }
    public IReadOnlyList<AssetContentDistribution> AssetContentDistributions {
        get => _assetContentDistributions.AsReadOnly();
    }

    ContentDistribution(
        DateOnly distributionDate,
        string distributionChannel,
        string distributionMethod,
        List<AssetContentDistribution> assetContentDistributions
    ) : base(ContentDistributionId.CreateUnique()) {
        DistributionDate = distributionDate;
        DistributionChannel = distributionChannel;
        DistributionMethod = distributionMethod;
        _assetContentDistributions = assetContentDistributions;
    }

    public static ContentDistribution Create(
        DateOnly distributionDate,
        string distributionChannel,
        string distributionMethod,
        List<AssetContentDistribution> assetContentDistributions
    ) {
        var contentDistribution = new ContentDistribution(distributionDate, distributionChannel, distributionMethod, assetContentDistributions);
        contentDistribution.AddDomainEvent(new ContentDistributionCreated(contentDistribution));
        return contentDistribution;
    }

#pragma warning disable CS8618
    ContentDistribution() { }
#pragma warning restore CS8618
}