using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Products;

namespace YGZ.Catalog.Application.ProductModels.Queries.GetProductsByCategory;

public sealed record GetProductModelsByCategoryQuery : IQuery<PaginationResponse<ProductModelResponse>>
{
    public required string CategorySlug { get; init; }
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public List<string>? Colors { get; init; }
    public List<string>? Storages { get; init; }
    public List<string>? Models { get; init; }
    public string? MinPrice { get; init; }
    public string? MaxPrice { get; init; }
    public string? PriceSort { get; init; }
}
