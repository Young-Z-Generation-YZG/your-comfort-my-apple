

namespace YGZ.BuildingBlocks.Shared.Contracts.Discounts;

public sealed record PromotionItemResponse
{
    public required string PromotionItemId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string PromotionEventType { get; init; }
    public required string DiscountState { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required string EndDiscountType { get; init; }
    public required string ProductNameTag { get; init; }
    public required DateTime? ValidFrom { get; init; }
    public required DateTime? ValidTo { get; init; }
    public required int? AvailableQuantity { get; init; }
    public required string ProductId { get; init; }
    public required string PromotionItemProductSlug { get; init; }
    public required string PromotionItemProductImage { get; init; }
}