

namespace YGZ.BuildingBlocks.Messaging.Events;

public record IntergrationEvent
{
    public Guid Id => Guid.NewGuid();
    public DateTime OccurredOn => DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName;
}
