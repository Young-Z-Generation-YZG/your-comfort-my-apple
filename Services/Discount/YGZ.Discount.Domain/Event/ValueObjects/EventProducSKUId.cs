using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.Event.ValueObjects;

public class EventProductSKUId : ValueObject
{
    public Guid? Value { get; set; }
    private EventProductSKUId(Guid? guid)
    {
        Value = guid;
    }

    public static EventProductSKUId Create()
    {
        return new EventProductSKUId(Guid.NewGuid());
    }

    public static EventProductSKUId Of(Guid? guid)
    {
        return new EventProductSKUId(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
