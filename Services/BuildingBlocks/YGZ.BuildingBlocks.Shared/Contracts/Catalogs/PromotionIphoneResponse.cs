
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record PromotionIphoneResponse
{
    //public string PromotionProductId { get; set; }
    required public string PromotionProductName { get; set; }
    public string PromotionProductDescription { get; set; } = string.Empty;
    public string PromotionProductImage { get; set; } = string.Empty;
    required public decimal PromotionProductUnitPrice { get; set; }
    required public string PromotionId { get; set; }
    required public string PromotionTitle { get; set; } = string.Empty;
    required public string PromotionEventType { get; set; }
    required public string PromotionDiscountType { get; set; }
    required public decimal PromotionDiscountValue { get; set; }
    required public decimal PromotionFinalPrice { get; set; }
    required public string PromotionProductSlug { get; set; }
    required public string CategoryId { get; set; }
    required public string ProductNameTag { get; set; }
    required public List<ProductVariantResponse> ProductVariants { get; set; } = new List<ProductVariantResponse>();
}

