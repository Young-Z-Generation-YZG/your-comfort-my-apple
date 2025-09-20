using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Domain.Event;

public class Event : AggregateRoot<EventId>, IAuditable, ISoftDelete
{
    public Event(EventId id) : base(id) { }

    public string Title { get; set; }
    public string Description { get; set; } = default!;
    public EState State { get; set; } = EState.INACTIVE;
    public DateTime? StartFrom { get; set; } = null;
    public DateTime? EndTo { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; private set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; private set; } = null;

    public static Event Create(EventId id,
                               string title,
                               string description,
                               EState state,
                               DateTime? startFrom,
                               DateTime? endTo)
    {
        return new Event(id)
        {
            Title = title,
            Description = description,
            State = state,
            StartFrom = startFrom,
            EndTo = endTo
        };
    }
}
