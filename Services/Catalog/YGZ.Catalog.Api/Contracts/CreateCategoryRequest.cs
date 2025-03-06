using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Contracts;

public sealed record CreateCategoryRequest(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("parent_id")] string? ParentId)
{ }