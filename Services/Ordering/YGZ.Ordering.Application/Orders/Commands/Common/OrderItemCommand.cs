namespace YGZ.Ordering.Application.Orders.Commands.Common;

public sealed record OrderItemCommand
{
    public required string ProductId { get; set; }
    public required string ModelId { get; set; }
    public required string ProductName { get; set; }
    public required string ProductColorName { get; set; }
    public required decimal ProductUnitPrice { get; set; }
    public required string ProductNameTag { get; set; }
    public required string ProductImage { get; set; }
    public required string ProductSlug { get; set; }
    public required int Quantity { get; set; }
    public required PromotionCommand? Promotion { get; set; }
}

public sealed record PromotionCommand
{
    public required string PromotionIdOrCode { get; set; }
    public required string PromotionEventType { get; set; }
    public required string PromotionTitle { get; set; }
    public required string PromotionDiscountType { get; set; }
    public required decimal PromotionDiscountValue { get; set; }
    public required decimal PromotionDiscountUnitPrice { get; set; }
    public required int PromotionAppliedProductCount { get; set; }
    public required decimal PromotionFinalPrice { get; set; }

}