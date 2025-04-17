

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record IPhonePromotionResponse
{
    required public string PromotionId { get; set; }
    required public string PromotionProductId { get; set; }
    public string? ProductModelId { get; set; } = null;
    required public string PromotionProductSlug { get; set; }
    required public string? PromotionTitle { get; set; } = null;
    required public string PromotionEventType { get; set; }
    required public string PromotionDiscountType { get; set; }
    required public decimal PromotionDiscountValue { get; set; }
    required public decimal PromotionFinalPrice { get; set; }
    required public string ProductNameTag { get; set; }
    required public string CategoryId { get; set; }
}
