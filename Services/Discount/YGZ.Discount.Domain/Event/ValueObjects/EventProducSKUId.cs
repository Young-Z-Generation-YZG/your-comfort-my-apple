using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.Event.ValueObjects;

public class EventProducSKUId : ValueObject
{
    public Guid? Value { get; set; }
    private EventProducSKUId(Guid? guid)
    {
        Value = guid;
    }

    public static EventProducSKUId Create()
    {
        return new EventProducSKUId(Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
