using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.PromotionRequest;

public sealed record UpdateEventRequest
{
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; init; }

    [JsonPropertyName("end_date")]
    public DateTime? EndDate { get; init; }

    [JsonPropertyName("add_event_items")]
    public List<UpdateEventItemRequest>? AddEventItems { get; init; }

    [JsonPropertyName("remove_event_item_ids")]
    public List<string>? RemoveEventItemIds { get; init; }
}

public sealed record UpdateEventItemRequest
{
    [Required]
    [JsonPropertyName("sku_id")]
    public required string SkuId { get; init; }

    [Required]
    [JsonPropertyName("discount_type")]
    public required string DiscountType { get; init; }

    [Required]
    [JsonPropertyName("discount_value")]
    public required decimal DiscountValue { get; init; }

    [Required]
    [JsonPropertyName("stock")]
    public required int Stock { get; init; }
}
