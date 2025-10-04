
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ColorResponse
{
    required public string ColorName { get; init; }
    required public string ColorHex { get; init; }
    required public string ColorImage { get; init; }
    public int? ColorOrder { get; init; }
}
