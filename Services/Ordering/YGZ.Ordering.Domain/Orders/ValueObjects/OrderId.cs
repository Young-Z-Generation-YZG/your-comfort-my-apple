
using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class OrderId : ValueObject
{
    public Guid Value { get; private set; }

    public OrderId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Order id cannot be empty", nameof(value));
        }

        Value = value;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static OrderId Create()
    {
        return new OrderId(Guid.NewGuid());
    }

    public static OrderId ToValueObject(Guid guid)
    {
        return new OrderId(guid);
    }
}
