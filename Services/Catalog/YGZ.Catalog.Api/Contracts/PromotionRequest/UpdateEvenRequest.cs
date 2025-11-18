using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.PromotionRequest;

public sealed record UpdateEvenRequest
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime? EndDate { get; set; }

    [JsonPropertyName("add_event_items")]
    public List<UpdateEventItemRequest>? AddEventItems { get; set; }

    [JsonPropertyName("remove_event_item_ids")]
    public List<string>? RemoveEventItemIds { get; set; }
}

public sealed record UpdateEventItemRequest
{
    [Required]
    [JsonPropertyName("sku_id")]
    required public string SkuId { get; set; }

    [Required]
    [JsonPropertyName("discount_type")]
    required public string DiscountType { get; set; }

    [Required]
    [JsonPropertyName("discount_value")]
    required public decimal DiscountValue { get; set; }

    [Required]
    [JsonPropertyName("stock")]
    required public int Stock { get; set; }
}
