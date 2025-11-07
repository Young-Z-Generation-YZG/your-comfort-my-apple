using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Products;

namespace YGZ.Catalog.Application.Products.Queries.GetProductModels;

public sealed record GetProductModelsQuery() : IQuery<PaginationResponse<ProductModelResponse>>
{
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public string? TextSearch { get; init; }
}
