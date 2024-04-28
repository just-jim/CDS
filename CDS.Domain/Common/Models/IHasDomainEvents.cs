namespace CDS.Domain.Common.Models;

public interface IHasDomainEvents {
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    public void ClearDomainEvents();
}