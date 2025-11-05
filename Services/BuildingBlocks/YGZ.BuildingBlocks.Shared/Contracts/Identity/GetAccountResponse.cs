
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Identity;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record GetAccountResponse
{
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string BirthDate { get; set; }
    public required string ImageId { get; set; }
    public required string ImageUrl { get; set; }
    public required string DefaultAddressLabel { get; set; }
    public required string DefaultContactName { get; set; }
    public required string DefaultContactPhoneNumber { get; set; }
    public required string DefaultAddressLine { get; set; }
    public required string DefaultAddressDistrict { get; set; }
    public required string DefaultAddressProvince { get; set; }
    public required string DefaultAddressCountry { get; set; }
}
