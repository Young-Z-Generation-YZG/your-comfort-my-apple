using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.Common;

public sealed record ModelRequest
{
    [Required]
    [JsonPropertyName("model_name")]
    public required string ModelName { get; init; }

    [JsonPropertyName("order")]
    public int Order { get; init; }
}
