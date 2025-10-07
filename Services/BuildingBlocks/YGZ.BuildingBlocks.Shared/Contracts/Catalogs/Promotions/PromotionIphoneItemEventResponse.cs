using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs.Promotions;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record PromotionIphoneItemEventResponse
{
    public required string EventProductSkuId { get; init; }
    public required string ModelId { get; init; }
    public required CategoryResponse Category { get; init; }
    public required ModelResponse Model { get; init; }
    public required ColorResponse Color { get; init; }
    public required StorageResponse Storage { get; init; }
    public required decimal OriginalPrice { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal DiscountAmount { get; init; }
    public required decimal FinalPrice { get; init; }
    public required int Stock { get; init; }
    public required int Sold { get; init; }
    public required string ImageUrl { get; init; }
    public required string ModelSlug { get; init; }
}
