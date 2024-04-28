namespace CDS.Domain.Common.Models.identities;

public abstract class AggregateRootId<TId> : EntityId<TId> {
    protected AggregateRootId(TId value) : base(value) {
    }
}