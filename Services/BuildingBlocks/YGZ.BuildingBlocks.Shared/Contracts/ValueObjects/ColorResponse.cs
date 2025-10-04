
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ColorResponse
{
    required public string Name { get; init; }
    required public string NormalizedName { get; init; }
    required public string HexCode { get; init; }
    required public string ShowcaseImageId { get; init; }
    required public int Order { get; init; }
}
