using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Products;

namespace YGZ.Catalog.Application.Products.Queries.GetSuggestionProducts;

public sealed record GetSuggestionProductsQuery : IQuery<PaginationResponse<SuggestionProductResponse>> { }