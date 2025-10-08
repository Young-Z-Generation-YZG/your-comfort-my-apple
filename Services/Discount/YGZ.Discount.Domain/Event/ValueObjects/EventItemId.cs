using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.Event.ValueObjects;

public class EventItemId : ValueObject
{
    public Guid? Value { get; set; }
    private EventItemId(Guid? guid)
    {
        Value = guid;
    }

    public static EventItemId Create()
    {
        return new EventItemId(Guid.NewGuid());
    }

    public static EventItemId Of(Guid? guid)
    {
        return new EventItemId(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
