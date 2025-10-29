

using System.ComponentModel.DataAnnotations.Schema;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

[ComplexType]
public class ShippingAddress : ValueObject
{
    public string ContactName { get; init; }
    public string ContactEmail { get; init; }
    public string ContactPhoneNumber { get; init; }
    public string AddressLine { get; init; }
    public string District { get; init; }
    public string Province { get; init; }
    public string Country { get; init; }

    private ShippingAddress(string contactName,
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

    public static ShippingAddress Create(string contactName,
                                         string contactEmail,
                                         string contactPhoneNumber,
                                         string addressLine,
                                         string district,
                                         string province,
                                         string country)
    {
        return new ShippingAddress(contactName,
                                   contactEmail,
                                   contactPhoneNumber,
                                   addressLine,
                                   district,
                                   province,
                                   country);
    }

    public static ShippingAddress Of(string contactName,
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

        return new ShippingAddress(contactName, contactEmail, contactPhoneNumber, addressLine, district, province, country);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ContactName;
        yield return ContactEmail;
        yield return ContactPhoneNumber;
        yield return AddressLine;
        yield return District;
        yield return Province;
        yield return Country;
    }

    public ShippingAddressResponse ToResponse()
    {
        return new ShippingAddressResponse
        {
            ContactName = ContactName,
            ContactEmail = ContactEmail,
            ContactPhoneNumber = ContactPhoneNumber,
            ContactAddressLine = AddressLine,
            ContactDistrict = District,
            ContactProvince = Province,
            ContactCountry = Country
        };
    }
}
