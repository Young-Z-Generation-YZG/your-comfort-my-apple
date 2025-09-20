using YGZ.Discount.Application.Abstractions;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Domain.Event;

public class Event : AggregateRoot<EventId>, IAuditable, ISoftDelete
{
    public Event(EventId id) : base(id) { }

    public required string Title { get; set; }
    public string Description { get; set; }
    public EState State { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; private set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; private set; } = null;

    public static Event Create(EventId id,
                               string title,
                               string? description,
                               DateTime? startDate,
                               DateTime? endDate)
    {
        return new Event(id)
        {
            Title = title,
            Description = description ?? String.Empty,
            State = EState.INACTIVE,
            StartDate = startDate,
            EndDate = endDate
        };
    }
}
