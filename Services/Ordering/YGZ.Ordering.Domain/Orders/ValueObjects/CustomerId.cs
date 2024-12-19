

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class CustomerId : ValueObject
{
    public Guid Value { get; private set; } = default!;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public CustomerId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Customer id cannot be empty", nameof(value));
        }

        Value = value;
    }

    public static CustomerId CreateNew()
    {
        return new CustomerId(Guid.NewGuid());
    }

    public static CustomerId Of(Guid guid)
    {
        if (guid == Guid.Empty)
        {
            throw new ArgumentException("Order id cannot be empty", nameof(guid));
        }

        return new CustomerId(guid);
    }
}
