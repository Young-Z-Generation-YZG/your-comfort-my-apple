
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

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
