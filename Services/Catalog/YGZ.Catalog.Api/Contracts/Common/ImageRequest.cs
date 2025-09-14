using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.Common;

public sealed record ImageRequest
{
    [Required]
    [JsonPropertyName("image_id")]
    public string ImageId { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("image_name")]
    public string ImageName { get; set; } = string.Empty;

    [JsonPropertyName("image_description")]
    public string ImageDescription { get; set; } = string.Empty;

    [JsonPropertyName("image_width")]
    public decimal ImageWidth { get; set; } = 0;

    [JsonPropertyName("image_height")]
    public decimal ImageHeight { get; set; } = 0;

    [JsonPropertyName("image_bytes")]
    public decimal ImageBytes { get; set; } = 0;

    [JsonPropertyName("order")]
    public int Order { get; init; }
}
