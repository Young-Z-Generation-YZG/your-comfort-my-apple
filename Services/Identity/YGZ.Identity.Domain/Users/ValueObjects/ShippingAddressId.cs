
using YGZ.Identity.Domain.Core.Exceptions;
using YGZ.Identity.Domain.Core.Primitives;

namespace YGZ.Identity.Domain.Users.ValueObjects;

public class ShippingAddressId : ValueObject
{
    public Guid Value { get; }

    private ShippingAddressId(Guid guid) => Value = guid;


    public static ShippingAddressId Create()
    {
        return new ShippingAddressId(Guid.NewGuid());
    }
        
    public static ShippingAddressId Of(Guid guid)
    {
        ArgumentNullException.ThrowIfNull(guid);

        if (guid == Guid.Empty)
        {
            throw new DomainException("ShippingAddressId cannot be empty.");
        }

        return new ShippingAddressId(guid);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
