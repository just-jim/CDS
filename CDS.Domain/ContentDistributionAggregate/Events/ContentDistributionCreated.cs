using CDS.Domain.Common.Models;

namespace CDS.Domain.ContentDistributionAggregate.Events;

public record ContentDistributionCreated(ContentDistribution ContentDistribution) : IDomainEvent;