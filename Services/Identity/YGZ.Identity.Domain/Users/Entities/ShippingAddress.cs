

using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Identity;
using YGZ.Identity.Domain.Core.Primitives;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Domain.Users.Entities;

public class ShippingAddress : Entity<ShippingAddressId>, IAuditable, ISoftDelete
{
    public ShippingAddress(ShippingAddressId id) : base(id)
    {
    }

    private ShippingAddress() : base(null!) { }

    required public string Label { get; set; }
    required public string ContactName { get; set; }
    required public string ContactPhoneNumber { get; set; }
    required public Address AddressDetail { get; set; }
    public bool IsDefault { get; set; } = false;
    required public string UserId { get; set; }
    public User User { get; set; } = null!;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;

    public static ShippingAddress Create(ShippingAddressId id,
                                         string label,
                                         string contactName,
                                         string contactPhoneNumber,
                                         Address addressDetail,
                                         bool isDefault,
                                         string userId)
    {
        ShippingAddress shippingAddress = new ShippingAddress(id)
        {
            Label = label,
            ContactName = contactName,
            ContactPhoneNumber = contactPhoneNumber,
            AddressDetail = addressDetail,
            IsDefault = isDefault,
            UserId = userId
        };

        return shippingAddress;
    }

    public void Update(string label,
                       string contactName,
                       string contactPhoneNumber,
                       Address addressDetail,
                       bool isDefault)
    {
        Label = label;
        ContactName = contactName;
        ContactPhoneNumber = contactPhoneNumber;
        AddressDetail = addressDetail;
        IsDefault = isDefault;
        UpdatedAt = DateTime.UtcNow;
    }

    public AddressResponse ToResponse()
    {
        return new AddressResponse
        {
            Id = Id.Value.ToString(),
            Label = Label,
            ContactName = ContactName,
            ContactPhoneNumber = ContactPhoneNumber,
            AddressLine = AddressDetail.AddressLine,
            District = AddressDetail.AddressDistrict,
            Province = AddressDetail.AddressProvince,
            Country = AddressDetail.AddressCountry,
            IsDefault = IsDefault
        };
    }
}
