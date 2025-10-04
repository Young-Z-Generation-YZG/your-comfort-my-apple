

using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ActivePromotionEventResponse
{
    public ActivePromotionEvent? PromotionEvent { get; init; } = null;
}

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record ActivePromotionEvent
{
    public string PromotionEventId { get; set; } = string.Empty;
    public string PromotionEventTitle { get; set; } = string.Empty;
    public string PromotionEventDescription { get; set; } = string.Empty;
    public string PromotionEventState { get; set; } = string.Empty;
    public DateTime PromotionEventValidFrom { get; set; }
    public DateTime PromotionEventValidTo { get; set; }
}