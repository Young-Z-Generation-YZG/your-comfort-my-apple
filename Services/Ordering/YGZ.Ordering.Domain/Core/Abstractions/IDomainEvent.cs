
using MediatR;

namespace YGZ.Ordering.Domain.Core.Abstractions;

public interface IDomainEvent : INotification
{
    Guid EventId => Guid.NewGuid();

    public DateTime OccurredOn => DateTime.Now;

    public string EventType => GetType().AssemblyQualifiedName!;
}