
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Identity;

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record AddressResponse()
{
    required public string Id { get; set; }
    required public string Label { get; set; }
    required public string ContactName { get; set; }
    required public string ContactPhoneNumber { get; set; }
    required public string AddressLine { get; set; }
    required public string District { get; set; }
    required public string Province { get; set; }
    required public string Country { get; set; }
    required public bool IsDefault { get; set; }
}
