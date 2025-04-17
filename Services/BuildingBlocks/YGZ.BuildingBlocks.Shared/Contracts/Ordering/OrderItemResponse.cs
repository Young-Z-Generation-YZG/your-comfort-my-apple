

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Ordering;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record OrderItemRepsonse()
{
    required public string OrderItemId { get; set; }
    required public string ProductId { get; set; }
    required public string ProductName { get; set; }
    required public string ProductImage { get; set; }
    required public string ProductColorName { get; set; }
    required public decimal ProductUnitPrice { get; set; }
    required public int Quantity { get; set; }
    public PromotionResponse? Promotion { get; set; } = null;
    required public decimal SubTotalAmount { get; set; }
    required public bool IsReviewed { get; set; }
}