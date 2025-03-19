using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.Common;

public sealed record ColorRequest
{
    [Required]
    [JsonPropertyName("color_name")]
    public string ColorName { get; set; } = string.Empty;

    [Required]
    [JsonPropertyName("color_hex")]
    public string ColorHex { get; set; } = string.Empty;

    [JsonPropertyName("color_order")]
    public int? ColorOrder { get; set; } = null;
}
