using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Iphone;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record IphoneModelDetailsResponse
{
    public required string Id { get; init; }
    public required CategoryResponse Category { get; init; }
    public required string Name { get; init; }
    public required List<ModelResponse> ModelItems { get; init; }
    public required List<ColorResponse> ColorItems { get; init; }
    public required List<StorageResponse> StorageItems { get; init; }
    public required List<SkuPriceListResponse> SkuPrices { get; init; }
    public string? Description { get; init; }
    public required List<ImageResponse> ShowcaseImages { get; init; }
    public required int OverallSold { get; init; }
    required public List<RatingStarResponse> RatingStars { get; init; }
    public required AverageRatingResponse AverageRating { get; init; }
    public required List<BranchWithSkusResponse> Branchs { get; init; }
}
