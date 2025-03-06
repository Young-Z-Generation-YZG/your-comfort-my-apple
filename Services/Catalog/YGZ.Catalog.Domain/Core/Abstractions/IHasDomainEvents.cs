

namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface IHasDomainEvents
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    public void ClearDomainEvents();
}
