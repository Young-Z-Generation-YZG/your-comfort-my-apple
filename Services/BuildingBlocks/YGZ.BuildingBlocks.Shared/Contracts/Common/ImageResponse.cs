

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record ImageResponse
{
    public string ImageId { get; init; }
    public string ImageUrl { get; init; }
    public string ImageName { get; init; }
    public string ImageDescription { get; init; }
    public decimal ImageWidth { get; init; }
    public decimal ImageHeight { get; init; }
    public decimal ImageBytes { get; init; }
    public int? ImageOrder { get; init; }
}
