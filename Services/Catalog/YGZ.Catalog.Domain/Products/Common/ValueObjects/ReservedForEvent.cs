using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class ReservedForEvent : ValueObject
{
    [BsonElement("event_id")]
    public string EventId { get; init; }

    [BsonElement("event_item_id")]
    public string EventItemId { get; init; }

    [BsonElement("event_name")]
    public string EventName { get; init; }
    
    [BsonElement("reserved_quantity")]
    public int ReservedQuantity { get; init; }


    private ReservedForEvent(string eventId, string eventItemId, string eventName, int reservedQuantity)
    {
        EventId = eventId;
        EventItemId = eventItemId;
        EventName = eventName;
        ReservedQuantity = reservedQuantity;
    }

    public static ReservedForEvent Create(string eventId, string eventItemId, string eventName, int reservedQuantity)
    {
        return new ReservedForEvent(eventId, eventItemId, eventName, reservedQuantity);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return EventId;
        yield return EventItemId;
        yield return EventName;
        yield return ReservedQuantity;
    }

    public ReservedForEventResponse ToResponse()
    {
        return new ReservedForEventResponse
        {
            EventId = EventId,
            EventItemId = EventItemId,
            EventName = EventName,
            ReservedQuantity = ReservedQuantity
        };
    }
}
