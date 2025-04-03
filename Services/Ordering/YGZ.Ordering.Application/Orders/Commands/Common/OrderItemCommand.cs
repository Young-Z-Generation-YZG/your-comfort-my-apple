namespace YGZ.Ordering.Application.Orders.Commands.Common;

#pragma warning disable CS8618

public sealed record OrderItemCommand
{
    public required string ProductId { get; set; }
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

    public string PromotionTitle { get; set; } = "UNKNOWN";

    public required string PromotionDiscountType { get; set; }

    public required decimal PromotionDiscountValue { get; set; }
}