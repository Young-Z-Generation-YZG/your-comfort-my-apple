using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts.CategoryRequest;

public sealed record CreateCategoryRequest
{
    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("parent_id")]
    public string? ParentId { get; init; }
}