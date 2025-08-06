

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record ImageResponse
{
    required public string ImageId { get; init; }
    required public string ImageUrl { get; init; }
    required public string ImageName { get; init; }
    required public string ImageDescription { get; init; }
    public decimal ImageWidth { get; init; }
    public decimal ImageHeight { get; init; }
    public decimal ImageBytes { get; init; }
    public int? ImageOrder { get; init; }
}
