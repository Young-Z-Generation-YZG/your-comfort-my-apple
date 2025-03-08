using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YGZ.Catalog.Api.Contracts.Common;

#pragma warning disable CS8618

namespace YGZ.Catalog.Api.Contracts;

public sealed record CreateProductItemRequest()
{
    [Required]
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [Required]
    [JsonPropertyName("color")]
    public ColorRequest Color { get; set; }

    [Required]
    [JsonPropertyName("storage")]
    public int Storage { get; set; }

    [Required]
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [Required]
    [JsonPropertyName("images")]
    public ImageRequest[] Images { get; set; }

    [Required]
    [JsonPropertyName("product_id")]
    public string ProductId { get; set; }
}