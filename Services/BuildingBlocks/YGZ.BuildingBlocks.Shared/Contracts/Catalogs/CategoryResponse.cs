
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record CategoryResponse
{
    required public string CategoryId { get; init; }
    required public string CategoryName { get; init; }
    required public string CategoryDescription { get; init; }
    required public int CategoryOrder { get; init; }
    required public string CategorySlug { get; init; }
    public string? CategoryParentId { get; init; }
}
