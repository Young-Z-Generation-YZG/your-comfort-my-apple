

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class Address : ValueObject
{
    public string ContactName { get; private set; } = default!;
    public string AddressLine { get; private set; } = default!;
    public string District { get; private set; } = default!;
    public string Province { get; private set; } = default!;
    public string Country { get; private set; } = default!;
    public string ContactEmail { get; private set; } = default!;
    public string ContactPhoneNumber { get; private set; } = default!;

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return AddressLine;
        yield return District;
        yield return Province;
        yield return Country;
        yield return ContactName;
        yield return ContactEmail;
        yield return ContactPhoneNumber;
    }

    public static Address CreateNew(
        string contactName,
        string contactEmail,
        string contactPhoneNumber,
        string addressLine,
        string district,
        string province,
        string country)
    {
        return new Address
        {
            AddressLine = addressLine,
            District = district,
            Province = province,
            Country = country,
            ContactName = contactName,
            ContactEmail = contactEmail,
            ContactPhoneNumber = contactPhoneNumber
        };
    }
}
