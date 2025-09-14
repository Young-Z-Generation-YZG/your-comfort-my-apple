using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YGZ.Catalog.Api.Contracts.Common;

namespace YGZ.Catalog.Api.Contracts.IphoneRequest;

public sealed record CreateIphoneModelRequest
{
    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [Required]
    [JsonPropertyName("models")]
    public required List<ModelRequest> Models { get; init; }

    [Required]
    [JsonPropertyName("colors")]
    public required List<ColorRequest> Colors { get; init; }

    [Required]
    [JsonPropertyName("storages")]
    public required List<StorageRequest> Storages { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("showcase_images")]
    public List<ImageRequest> ShowcaseImages { get; init; } = [];

    [Required]
    [JsonPropertyName("category_id")]
    public required string CategoryId { get; init; }
}
