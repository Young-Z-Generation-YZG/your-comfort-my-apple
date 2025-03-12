

using System.ComponentModel.DataAnnotations.Schema;
using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

[ComplexType]
public class Address : ValueObject
{
    public string ContactName { get; private set; } = default!;
    public string ContactEmail { get; private set; } = default!;
    public string ContactPhoneNumber { get; private set; } = default!;
    public string AddressLine { get; private set; } = default!;
    public string District { get; private set; } = default!;
    public string Province { get; private set; } = default!;
    public string Country { get; private set; } = default!;

    private Address(string contactName,
                    string contactEmail,
                    string contactPhoneNumber,
                    string addressLine,
                    string district,
                    string province,
                    string country)
    {
        ContactName = contactName;
        ContactEmail = contactEmail;
        ContactPhoneNumber = contactPhoneNumber;
        AddressLine = addressLine;
        District = district;
        Province = province;
        Country = country;
    }

    public static Address Create(string contactName,
                                 string contactEmail,
                                 string contactPhoneNumber,
                                 string addressLine,
                                 string district,
                                 string province,
                                 string country)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contactName);
        ArgumentException.ThrowIfNullOrWhiteSpace(contactEmail);
        ArgumentException.ThrowIfNullOrWhiteSpace(contactPhoneNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);
        ArgumentException.ThrowIfNullOrWhiteSpace(district);
        ArgumentException.ThrowIfNullOrWhiteSpace(province);
        ArgumentException.ThrowIfNullOrWhiteSpace(country);

        return new Address(contactName, contactEmail, contactPhoneNumber, addressLine, district, province, country);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}
