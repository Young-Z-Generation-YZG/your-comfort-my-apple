

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class Address : ValueObject
{
    public string AddressLine { get; private set; } = default!;
    public string District { get; private set; } = default!;
    public string Province { get; private set; } = default!;
    public string City { get; private set; } = default!;
    public string Country { get; private set; } = default!;
    public string ContactName { get; private set; } = default!;
    public string ContactEmail { get; private set; } = default!;
    public string ContactPhoneNumber { get; private set; } = default!;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return AddressLine;
        yield return District;
        yield return Province;
        yield return City;
        yield return Country;
        yield return ContactName;
        yield return ContactEmail;
        yield return ContactPhoneNumber;
    }

    public static Address CreateNew(
        string addressLine,
        string district,
        string province,
        string city,
        string country,
        string contactName,
        string contactEmail,
        string contactPhoneNumber)
    {
        return new Address
        {
            AddressLine = addressLine,
            District = district,
            Province = province,
            City = city,
            Country = country,
            ContactName = contactName,
            ContactEmail = contactEmail,
            ContactPhoneNumber = contactPhoneNumber
        };
    }
}
