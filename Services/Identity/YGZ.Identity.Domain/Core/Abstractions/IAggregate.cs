
namespace YGZ.Identity.Domain.Core.Abstractions;

public interface IAggregate
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    public void AddDomainEvent(IDomainEvent domainEvent);

    public IDomainEvent[] ClearDomainEvents();
}