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
    [JsonPropertyName("image_order")]
    public int ImageOrder { get; set; } = 0;
}
