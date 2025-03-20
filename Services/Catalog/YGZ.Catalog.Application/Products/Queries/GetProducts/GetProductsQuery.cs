

using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;

namespace YGZ.Catalog.Application.Products.Queries.GetProductsPagination;

public sealed class GetProductsQuery() : IQuery<PaginationResponse<ProductResponse>>
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public string? ProductColor { get; set; }
    public string? ProductStorage { get; set; }
    public string? PriceFrom { get; set; }
    public string? PriceTo { get; set; }
    public string? PriceSort { get; set; }
}