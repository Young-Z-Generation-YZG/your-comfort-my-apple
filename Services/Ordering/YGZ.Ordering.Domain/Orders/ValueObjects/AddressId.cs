

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class AddressId : ValueObject
{
    public Guid Value { get; private set; }

    public AddressId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("AddressId cannot be empty", nameof(value));
        }

        Value = value;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static AddressId Create()
    {
        return new AddressId(Guid.NewGuid());
    }

    public static AddressId ToValueObject(Guid guid)
    {
        return new AddressId(guid);
    }
}
