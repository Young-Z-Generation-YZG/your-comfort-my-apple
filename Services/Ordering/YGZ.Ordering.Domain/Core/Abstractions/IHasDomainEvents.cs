

namespace YGZ.Ordering.Domain.Core.Abstractions;

public interface IHasDomainEvents
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    public void ClearDomainEvents();
}