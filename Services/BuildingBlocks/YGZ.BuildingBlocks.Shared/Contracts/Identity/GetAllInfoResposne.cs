
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Identity;

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record GetAllInfoResposne
{
    required public string Email { get; set; }
    required public string FirstName { get; set; }
    required public string LastName { get; set; }
    required public string PhoneNumber { get; set; }
    required public string BirthDate { get; set; }
    required public string ImageId { get; set; }
    required public string ImageUrl { get; set; }
    required public string DefaultAddressLabel { get; set; }
    required public string DefaultContactName { get; set; }
    required public string DefaultContactPhoneNumber { get; set; }
    required public string DefaultAddressLine { get; set; }
    required public string DefaultAddressDistrict { get; set; }
    required public string DefaultAddressProvince { get; set; }
    required public string DefaultAddressCountry { get; set; }
}
