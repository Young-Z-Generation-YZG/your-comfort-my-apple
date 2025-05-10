

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Ordering;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record OrderItemRepsonse()
{
    public required string OrderId { get; set; }
    public required string OrderItemId { get; set; }
    public required string ProductId { get; set; }
    public required string ModelId { get; set; }
    public required string ProductName { get; set; }
    public required string ProductImage { get; set; }
    public required string ProductColorName { get; set; }
    public required decimal ProductUnitPrice { get; set; }
    public required int Quantity { get; set; }
    public PromotionResponse? Promotion { get; set; } = null;
    public required decimal SubTotalAmount { get; set; }
    public required bool IsReviewed { get; set; }
}