

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class OrderLineId : ValueObject
{
    public Guid Value { get; private set; }
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public OrderLineId(Guid value)
    {
        Value = value;
    }

    public static OrderLineId CreateNew()
    {
        return new OrderLineId(Guid.NewGuid());
    }
}
