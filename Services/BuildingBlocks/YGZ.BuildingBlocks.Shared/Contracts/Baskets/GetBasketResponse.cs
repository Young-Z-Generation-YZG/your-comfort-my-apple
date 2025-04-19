
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record GetBasketResponse()
{
    required public string UserEmail { get; set; } = default!;
    required public List<CartItemResponse> CartItems { get; set; } = new List<CartItemResponse>();
    required public decimal TotalAmount { get; set; } = 0;
}

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record CartItemResponse()
{
    required public string ProductId { get; set; }
    required public string ModelId { get; set; }
    required public string ProductName { get; set; }
    required public string ProductColorName { get; set; }
    required public decimal ProductUnitPrice { get; set; }
    required public string ProductNameTag { get; set; }
    required public string ProductImage { get; set; }
    required public string ProductSlug { get; set; }
    required public string CategoryId { get; set; }
    required public int Quantity { get; set; }
    required public decimal SubTotalAmount { get; set; }
    public PromotionResponse? Promotion { get; set; } = null;
    required public int OrderIndex { get; set; } = 0;
}

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record class PromotionResponse()
{
    public string PromotionIdOrCode { get; set; }
    public string PromotionEventType { get; set; }
    public string PromotionTitle { get; set; }
    public string PromotionDiscountType { get; set; }
    public decimal PromotionDiscountValue { get; set; }
    public decimal PromotionDiscountUnitPrice { get; set; }
    public int PromotionAppliedProductCount { get; set; }
    public decimal PromotionFinalPrice { get; set; }
}