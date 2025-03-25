
namespace YGZ.BuildingBlocks.Shared.Contracts.Discounts;

public class PromotionGlobalEventResponse
{
    public PromotionEventResponse promotionEvent { get; set; }
    public List<PromotionProductResponse> PromotionProducts { get; set; }
    public List<PromotionCategoryResponse> PromotionCategories { get; set; }
}

public sealed record PromotionEventResponse
{
    public string? PromotionEventId { get; set; }
    public string PromotionEventTitle { get; set; }
    public string PromotionEventDescription { get; set; }
    public string PromotionEventState { get; set; }
    public DateTime? PromotionEventValidFrom { get; set; }
    public DateTime? PromotionEventValidTo { get; set; }
}

public sealed record PromotionProductResponse
{
    public string ProductId { get; set; }
    public string ProductColorName { get; set; }
    public string ProductStorage { get; set; }
    public string ProductImage { get; set; }
    public string ProductSlug { get; set; }
    public decimal DiscountPercent { get; set; }
}

public sealed record PromotionCategoryResponse
{
    public string CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string CategorySlug { get; set; }
    public decimal DiscountPercent { get; set; }
}
