
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record CategoryResponse
{
    required public string Id { get; init; }
    required public string Name { get; init; }
    required public string Description { get; init; }
    required public int Order { get; init; }
    required public string Slug { get; init; }
    public string? ParentId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? ModifiedBy { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
}
