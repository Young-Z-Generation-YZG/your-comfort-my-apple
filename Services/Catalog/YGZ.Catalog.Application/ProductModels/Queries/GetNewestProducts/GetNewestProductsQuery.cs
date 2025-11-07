using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Products;

namespace YGZ.Catalog.Application.Products.Queries.GetRecommendationProducts;

public sealed record GetNewestProductsQuery() : IQuery<PaginationResponse<NewestProductResponse>> { }
