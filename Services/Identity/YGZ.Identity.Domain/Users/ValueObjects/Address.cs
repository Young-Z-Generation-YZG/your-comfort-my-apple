

using YGZ.Identity.Domain.Core.Primitives;

namespace YGZ.Identity.Domain.Users.ValueObjects;

public class Address : ValueObject
{
    public string AddressLine { get; set; } = default!;
    public string AddressDistrict { get; set; } = default!;
    public string AddressProvince { get; set; } = default!;
    public string AddressCountry { get; set; } = default!;

    public static Address Create(string? addressLine, string? addressDistrict, string? addressProvince, string? addressCountry)
    {
        return new Address
        {
            AddressLine = addressLine!,
            AddressDistrict = addressDistrict!,
            AddressProvince = addressProvince!,
            AddressCountry = addressCountry!
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return AddressLine;
        yield return AddressDistrict;
        yield return AddressProvince;
        yield return AddressCountry;
    }
}
