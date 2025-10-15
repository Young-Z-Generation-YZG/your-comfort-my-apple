using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Ordering;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ShippingAddressResponse
{
    public required string ContactName { get; init; }
    public required string ContactEmail { get; init; }
    public required string ContactPhoneNumber { get; init; }
    public required string ContactAddressLine { get; init; }
    public required string ContactDistrict { get; init; }
    public required string ContactProvince { get; init; }
    public required string ContactCountry { get; init; }
}