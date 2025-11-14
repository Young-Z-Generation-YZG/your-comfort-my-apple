
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record CategoryResponse
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; } = null;
    public required int Order { get; init; }
    public required string Slug { get; init; }
    public CategoryResponse? ParentCategory { get; init; } = null;
    public List<CategoryResponse>? SubCategories { get; init; } = null;
    public List<ProductModelResponse>? ProductModels { get; init; } = null;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
}
