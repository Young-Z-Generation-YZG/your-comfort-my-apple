

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; private set; }

    public UserId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty", nameof(value));
        }

        Value = value;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static UserId Create()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId ToValueObject(Guid guid)
    {
        return new UserId(guid);
    }
}
