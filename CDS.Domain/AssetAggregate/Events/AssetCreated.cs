using CDS.Domain.Common.Models;

namespace CDS.Domain.AssetAggregate.Events;

public record AssetCreated(Asset Asset) : IDomainEvent;
public record AssetUpdated(Asset Asset) : IDomainEvent;