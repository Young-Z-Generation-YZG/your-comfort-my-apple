
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Discounts;

[JsonConverter(typeof(SnakeCaseSerializer))]
public class PromotionGlobalEventResponse
{
    public PromotionEventResponse promotionEvent { get; set; }
    public List<PromotionProductResponse> PromotionProducts { get; set; }
    public List<PromotionCategoryResponse> PromotionCategories { get; set; }
}

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record PromotionEventResponse
{
    required public string? PromotionEventId { get; set; }
    required public string PromotionEventTitle { get; set; }
    public string PromotionEventDescription { get; set; } = string.Empty;
    required public string PromotionEventType { get; set; }
    required public string PromotionEventState { get; set; }
    public DateTime? PromotionEventValidFrom { get; set; }
    public DateTime? PromotionEventValidTo { get; set; }
}

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record PromotionProductResponse
{
    required public string ProductId { get; set; }
    required public string ProductSlug { get; set; }
    public string ProductImage { get; set; } = string.Empty;
    required public string DiscountType { get; set; }
    required public decimal DiscountValue { get; set; }
    required public string PromotionGlobalId { get; set; }
}

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record PromotionCategoryResponse
{
    required public string CategoryId { get; set; }
    required public string CategoryName { get; set; }
    required public string CategorySlug { get; set; }
    required public string DiscountType { get; set; }
    required public decimal DiscountValue { get; set; }
    required public string PromotionGlobalId { get; set; }
}
