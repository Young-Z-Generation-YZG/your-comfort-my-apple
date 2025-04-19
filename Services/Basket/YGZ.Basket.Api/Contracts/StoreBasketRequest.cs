using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Basket.Api.Contracts;

#pragma warning disable CS8618

public sealed record StoreBasketRequest
{
    [Required]
    [JsonPropertyName("cart_items")]
    public List<CartItemRequest> CartItems { get; set; }
}

public sealed record CartItemRequest()
{
    [Required]
    [JsonPropertyName("product_id")]
    required public string ProductId { get; set; }

    [Required]
    [JsonPropertyName("model_id")]
    required public string ModelId { get; set; }

    [Required]
    [JsonPropertyName("product_name")]
    public string ProductName { get; set; }

    [Required]
    [JsonPropertyName("product_color_name")]
    public string ProductColorName { get; set; }

    [Required]
    [JsonPropertyName("product_unit_price")]
    public decimal ProductUnitPrice { get; set; }

    [Required]
    [JsonPropertyName("product_name_tag")]
    public string ProductNameTag { get; set; }

    [Required]
    [JsonPropertyName("product_image")]
    public string ProductImage { get; set; }

    [Required]
    [JsonPropertyName("product_slug")]
    public string ProductSlug { get; set; }

    [Required]
    [JsonPropertyName("category_id")]
    public string CategoryId { get; set; }

    [Required]
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("promotion")]
    public PromotionRequest? Promotion { get; set; }

    [JsonPropertyName("order_index")]
    public int OrderIndex { get; set; }
}

public sealed record PromotionRequest()
{
    [Required]
    [JsonPropertyName("promotion_id_or_code")]
    public required string PromotionIdOrCode { get; set; }

    [Required]
    [JsonPropertyName("promotion_event_type")]
    public required string PromotionEventType { get; set; }
}