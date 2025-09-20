
using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.Event.ValueObjects;

public class EventId : ValueObject
{
    public Guid? Value { get; set; }
    private EventId(Guid? guid)
    {
        Value = guid;
    }

    public static EventId Create()
    {
        return new EventId(Guid.NewGuid());
    }

    public static EventId Of(Guid? guid)
    {
        return new EventId(guid);
    }

    public static EventId Of(string guid)
    {
        Guid.TryParse(guid, out var parsedGuid);

        if (parsedGuid == Guid.Empty)
        {
            throw new ArgumentException("Invalid GUID format", nameof(guid));
        }

        return new EventId(parsedGuid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
