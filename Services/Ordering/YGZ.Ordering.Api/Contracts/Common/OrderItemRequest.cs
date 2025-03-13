using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Ordering.Api.Contracts.Common;

#pragma warning disable CS8618

public sealed record OrderItemRequest
{
    [Required]
    [JsonPropertyName("product_id")]
    public string ProductId { get; set; }

    [Required]
    [JsonPropertyName("product_model")]
    public string ProductModel { get; set; }

    [Required]
    [JsonPropertyName("product_color")]
    public string ProductColor { get; set; }

    [Required]
    [JsonPropertyName("product_color_hex")]
    public string ProductColorHex { get; set; }

    [Required]
    [JsonPropertyName("product_storage")]
    public int ProductStorage { get; set; }

    [Required]
    [JsonPropertyName("product_price")]
    public decimal ProductPrice { get; set; }

    [Required]
    [JsonPropertyName("product_image")]
    public string ProductImage { get; set; }

    [Required]
    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
