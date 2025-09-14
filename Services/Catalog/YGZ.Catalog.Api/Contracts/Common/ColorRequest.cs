using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.Common;

public sealed record ColorRequest
{
    [Required]
    [JsonPropertyName("color_name")]
    public required string ColorName { get; init; }

    [Required]
    [JsonPropertyName("color_hex_code")]
    public required string ColorHexCode { get; init; }

    [JsonPropertyName("showcase_image_id")]
    public string? ShowcaseImageId { get; init; } = null;

    [JsonPropertyName("order")]
    public int Order { get; init; }
}
