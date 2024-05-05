using CDS.Domain.Common.Models;

namespace CDS.Domain.AssetAggregate.Events;

public record AssetUpdated(Asset Asset) : IDomainEvent;