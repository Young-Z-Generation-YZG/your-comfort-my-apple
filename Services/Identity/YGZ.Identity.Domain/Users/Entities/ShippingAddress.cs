

using YGZ.Identity.Domain.Core.Abstractions;
using YGZ.Identity.Domain.Core.Primitives;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Domain.Users.Entities;

public class ShippingAddress : Entity<ShippingAddressId>, IAuditable
{
    public ShippingAddress(ShippingAddressId id) : base(id)
    {
    }

    private ShippingAddress() : base(null!) { }

    required public string ContactName { get; set; } 
    required public string ContactPhoneNumber { get; set; }
    required public Address AddressDetail { get; set; }
    public bool IsDefault { get; set; } = false;
    required public string UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public static ShippingAddress Create(ShippingAddressId id,
                                         string contactName,
                                         string contactPhoneNumber,
                                         Address addressDetail,
                                         bool isDefault,
                                         string userId)
    {
        ShippingAddress shippingAddress = new ShippingAddress(id)
        {
            ContactName = contactName,
            ContactPhoneNumber = contactPhoneNumber,
            AddressDetail = addressDetail,
            IsDefault = isDefault,
            UserId = userId
        };

        return shippingAddress;
    }
}
