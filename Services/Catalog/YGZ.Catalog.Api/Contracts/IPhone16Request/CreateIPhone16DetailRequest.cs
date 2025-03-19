using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YGZ.Catalog.Api.Contracts.Common;

namespace YGZ.Catalog.Api.Contracts.IPhone16;

#pragma warning disable CS8618

public sealed record CreateIPhone16DetailRequest()
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
    [JsonPropertyName("unit_price")]
    public decimal UnitPrice { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [Required]
    [JsonPropertyName("images")]
    public ImageRequest[] Images { get; set; }

    [Required]
    [JsonPropertyName("iphone_model_id")]
    public string IPhoneModelId { get; set; }
}