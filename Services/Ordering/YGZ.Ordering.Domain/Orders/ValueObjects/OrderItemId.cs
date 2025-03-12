

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class OrderItemId : ValueObject
{
    public Guid Value { get; private set; }

    public OrderItemId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Order item id cannot be empty", nameof(value));
        }

        Value = value;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static OrderItemId Create()
    {
        return new OrderItemId(Guid.NewGuid());
    }

    public static OrderItemId ToValueObject(Guid guid)
    {
        return new OrderItemId(guid);
    }
}
