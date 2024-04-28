using CDS.Domain.Common.Models;

namespace CDS.Domain.OrderAggregate.Events;

public record OrderCreated(Order Order) : IDomainEvent;