
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record GetBasketResponse()
{
    required public string UserEmail { get; set; } = default!;
    required public List<CartItemResponse> CartItems { get; set; } = new List<CartItemResponse>();
    required public decimal TotalAmount { get; set; }
}

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record CartItemResponse() 
{
    public required string ProductId { get; set; } 
    public required string ProductName { get; set; } 
    public required string ProductColorName { get; set; } 
    public required decimal ProductUnitPrice { get; set; }
    public required string ProductNameTag { get; set; } 
    public required string ProductImage { get; set; }
    public required string ProductSlug { get; set; }
    public required int Quantity { get; set; }
    public required decimal SubTotalAmount { get; set; }
    public PromotionResponse? Promotion { get; set; }
    public required int OrderIndex { get; set; } = 0;
}

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record class PromotionResponse()
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