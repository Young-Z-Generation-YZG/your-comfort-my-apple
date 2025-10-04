

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ModelResponse
{
    required public string Name { get; set; }
    required public string NormalizedName { get; set; }
    required public int Order { get; set; }
}
