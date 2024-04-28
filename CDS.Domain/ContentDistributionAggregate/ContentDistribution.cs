using CDS.Domain.Common.Models;
using CDS.Domain.ContentDistributionAggregate.Events;
using CDS.Domain.ContentDistributionAggregate.ValueObjects;

namespace CDS.Domain.ContentDistributionAggregate;

public class ContentDistribution : AggregateRoot<ContentDistributionId, Guid> {
    public DateTime DistributionDate { get; private set; }
    public string DistributionChannel { get; private set; }
    public string DistributionMethod { get; private set; }

    ContentDistribution(
        DateTime distributionDate, 
        string distributionChannel, 
        string distributionMethod
    ) : base(ContentDistributionId.CreateUnique()) {
        DistributionDate = distributionDate;
        DistributionChannel = distributionChannel;
        DistributionMethod = distributionMethod;
    }

    public static ContentDistribution Create(
        DateTime distributionDate, 
        string distributionChannel, 
        string distributionMethod
    ) {
        var contentDistribution = new ContentDistribution(distributionDate, distributionChannel, distributionMethod);
        contentDistribution.AddDomainEvent(new ContentDistributionCreated(contentDistribution));
        return contentDistribution;
    }
}