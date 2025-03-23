
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Catalogs;

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record CategoryResponse
{
    public string CategoryId { get; init; }
    public string CategoryName { get; init; }
    public string CategoryDescription { get; init; }
    public int CategoryOrder { get; init; }
    public string CategorySlug { get; init; }
    public string? CategoryParentId { get; init; }
}
