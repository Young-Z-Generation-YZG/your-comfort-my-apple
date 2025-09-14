using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.Common;

public sealed record StorageRequest
{
    [Required]
    [JsonPropertyName("storage_name")]
    public required string StorageName { get; init; }

    [Required]
    [JsonPropertyName("storage_value")]
    public required int StorageValue { get; init; }

    [JsonPropertyName("order")]
    public int Order { get; init; }
}
