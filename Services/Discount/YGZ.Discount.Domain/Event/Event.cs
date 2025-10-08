using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Core.Primitives;
using YGZ.Discount.Domain.Event.Entities;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Domain.Event;

public class Event : AggregateRoot<EventId>, IAuditable, ISoftDelete
{
    public Event(EventId id) : base(id) { }

    public required string Title { get; set; }
    public required string Description { get; set; }
    public required EState State { get; set; }
    public DateTime? StartDate { get; set; } = null;
    public DateTime? EndDate { get; set; } = null;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; private set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; private set; } = null;
    private readonly List<EventItem> _eventItems = new();

    // Navigation property for EventItems
    public IReadOnlyList<EventItem> EventItems => _eventItems.AsReadOnly();

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

    // Methods to manage EventItems
    public void AddEventItem(EventItem eventItem)
    {
        if (eventItem.EventId != Id)
        {
            throw new ArgumentException("EventItem must belong to this Event");
        }

        var existingItem = _eventItems.FirstOrDefault(ep =>
            ep.NormalizedModel == eventItem.NormalizedModel &&
            ep.NormalizedColor == eventItem.NormalizedColor &&
            ep.NormalizedStorage == eventItem.NormalizedStorage &&
            !ep.IsDeleted);

        if (existingItem is not null)
        {
            throw new ArgumentException("EventItem already exists for this event");
        }

        _eventItems.Add(eventItem);
    }

    public void RemoveEventItem(EventItem eventItem)
    {
        _eventItems.Remove(eventItem);
    }
}
