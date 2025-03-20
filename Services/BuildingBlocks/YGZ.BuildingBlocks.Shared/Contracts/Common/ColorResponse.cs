
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record ColorResponse
{
    public string ColorName { get; init; }
    public string ColorHex { get; init; }
    public string ColorImage { get; init; }
    public int? ColorOrder { get; init; }
}
