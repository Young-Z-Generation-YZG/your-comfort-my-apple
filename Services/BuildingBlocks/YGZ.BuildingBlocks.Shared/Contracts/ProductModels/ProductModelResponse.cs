using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Products;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ProductModelResponse
{
    public required string Id { get; init; }
    public required CategoryResponse Category { get; init; }
    public required string Name { get; init; }
    public required string NormalizedModel { get; init; }
    public required string ProductClassification { get; init; }
    public required List<ModelResponse> ModelItems { get; init; }
    public required List<ColorResponse> ColorItems { get; init; }
    public required List<StorageResponse> StorageItems { get; init; }
    public required List<SkuPriceListResponse> SkuPrices { get; init; }
    public required List<ImageResponse> ShowcaseImages { get; init; }
    public string? Description { get; init; }
    public required AverageRatingResponse AverageRating { get; init; }
    public required List<RatingStarResponse> RatingStars { get; init; }
    public required int OverallSold { get; init; }
    public PromotionResponse? Promotion { get; init; }
    public bool IsNewest { get; init; }
    public required string Slug { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
}


[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record SkuPriceListResponse
{
    public required string NormalizedModel { get; init; }
    public required string NormalizedColor { get; init; }
    public required string NormalizedStorage { get; init; }
    public required decimal UnitPrice { get; init; }
}