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
    private readonly List<EventProductSKU> _eventProductSKUs = new();
    
    // Navigation property for EventProductSKUs
    public IReadOnlyList<EventProductSKU> EventProductSKUs => _eventProductSKUs.AsReadOnly();

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

    // methods to manage EventProductSKUs
    public void AddEventProductSKU(EventProductSKU eventProductSKU)
    {
        if (eventProductSKU.EventId != Id)
        {
            throw new ArgumentException("EventProductSKU must belong to this Event");
        }

        var existingSKU = _eventProductSKUs.FirstOrDefault(ep => ep.SKUId == eventProductSKU.SKUId && !ep.IsDeleted);

        if (existingSKU is not null)
        {
            throw new ArgumentException("EventProductSKU already exists for this event");
        }

        _eventProductSKUs.Add(eventProductSKU);
    }

    public void RemoveEventProductSKU(EventProductSKU eventProductSKU)
    {
        _eventProductSKUs.Remove(eventProductSKU);
    }

    public EventProductSKU? GetEventProductSKUBySKUId(string skuId)
    {
        return _eventProductSKUs.FirstOrDefault(ep => ep.SKUId == skuId && !ep.IsDeleted);
    }
}
