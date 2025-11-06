using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Products;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record SuggestionProductResponse
{
    public required string Id { get; init; }
    public required CategoryResponse Category { get; init; }
    public required string Name { get; init; }
    public required string NormalizedModel { get; init; }
    public required string ProductClassification { get; init; }
    public required List<ModelResponse> ModelItems { get; init; }
    public required List<ColorResponse> ColorItems { get; init; }
    public required List<StorageResponse> StorageItems { get; init; }
    public required List<IphoneSkuPriceListResponse> SkuPrices { get; init; }
    public string? Description { get; init; }
    public required List<ImageResponse> ShowcaseImages { get; init; }
    public required int OverallSold { get; init; }
    required public List<RatingStarResponse> RatingStars { get; init; }
    public required AverageRatingResponse AverageRating { get; init; }
    public bool IsNewest { get; init; }
    public required string Slug { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
}
