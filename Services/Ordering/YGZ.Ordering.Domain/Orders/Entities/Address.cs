

using YGZ.Ordering.Domain.Core.Primitives;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Domain.Orders.Entities;

public class Address : Entity<AddressId>
{
    public Address(AddressId id) : base(id)
    {
    }

    private Address(AddressId id,
                    string contactName,
                    string contactEmail,
                    string contactPhoneNumber,
                    string addressLine,
                    string district,
                    string province,
                    string country) : base(id)
    {
        ContactName = contactName;
        ContactEmail = contactEmail;
        ContactPhoneNumber = contactPhoneNumber;
        AddressLine = addressLine;
        District = district;
        Province = province;
        Country = country;
    }

    public string? ContactName { get; private set; } = default!;
    public string? ContactEmail { get; private set; } = default!;
    public string? ContactPhoneNumber { get; private set; } = default!;
    public string? AddressLine { get; private set; } = default!;
    public string? District { get; private set; } = default!;
    public string? Province { get; private set; } = default!;
    public string? Country { get; private set; } = default!;
}
