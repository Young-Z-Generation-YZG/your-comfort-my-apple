using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record EmbeddedSkuResponse
{
    public required string SkuId { get; init; }
    public required string ModelNormalizedName { get; init; }
    public required string ColorNormalizedName { get; init; }
    public required string StorageNormalizedName { get; init; }
    public required string ImageUrl { get; init; }
}
