

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Common;

[JsonConverter(typeof(SnakeCaseSerializer))]
public sealed record ModelPromotionResponse
{
    public required decimal MinimumPromotionPrice { get; set; }
    public required decimal MaximumPromotionPrice { get; set; }
    public required decimal MinimumDiscountPercentage { get; set; }
    public required decimal MaximumDiscountPercentage { get; set; }
}
