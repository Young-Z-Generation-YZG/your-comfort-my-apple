

namespace YGZ.Ordering.Domain.Core.Abstractions;

public interface IHasDomainEvents
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    public void AddDomainEvent(IDomainEvent domainEvent);

    public IDomainEvent[] ClearDomainEvents();
}