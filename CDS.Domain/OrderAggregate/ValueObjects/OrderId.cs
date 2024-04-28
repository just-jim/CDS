using CDS.Domain.Common.Models.identities;

namespace CDS.Domain.OrderAggregate.ValueObjects;

public sealed class OrderId : AggregateRootId<string> {
    OrderId(string value) : base(value) {
    }

    public static OrderId Create(string value) {
        return new OrderId(value);
    }
}