using YGZ.Identity.Domain.Core.Primitives;

namespace YGZ.Identity.Domain.Users.ValueObjects;

public class ShippingAddressId : ValueObject
{
    public Guid Value { get; private set; }

    private ShippingAddressId(Guid guid)
    {
        Value = guid;
    }

    public static ShippingAddressId Create()
    {
        return new ShippingAddressId(Guid.NewGuid());
    }

    public static ShippingAddressId Of(Guid guid)
    {
        return new ShippingAddressId(guid);
    }

    public static ShippingAddressId Of(string guid)
    {
        Guid.TryParse(guid, out var parsedGuid);

        if (parsedGuid == Guid.Empty)
        {
            throw new ArgumentException("Invalid GUID format", nameof(parsedGuid));
        }

        return new ShippingAddressId(parsedGuid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
