
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;

namespace YGZ.Catalog.Application.Products.Queries.GetProductsPagination;

public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, bool>
{
    private readonly IMongoRepository<IPhone16Detail> _iPhone16repository;

    public GetProductsQueryHandler(IMongoRepository<IPhone16Detail> iPhone16repository)
    {
        _iPhone16repository = iPhone16repository;
    }

    public async Task<Result<bool>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var (filter, sort) = GetFilterDefinition(request);

        var result = await _iPhone16repository.GetAllAsync(request.Page, request.Limit, filter, sort, cancellationToken);

        var response = MapToResponse(result.items, result.totalRecords, result.totalPages, request);

        return true;
    }

    private (FilterDefinition<IPhone16Detail> filterBuilder, SortDefinition<IPhone16Detail> sort) GetFilterDefinition(GetProductsQuery request)
    {
        var filterBuilder = Builders<IPhone16Detail>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrEmpty(request.ProductColor))
        {
            filter &= filterBuilder.Eq(x => x.Color.ColorName, request.ProductColor);
        }

        if (!string.IsNullOrEmpty(request.ProductStorage))
        {
            int.TryParse(request.ProductStorage, out var storageValue);

            if(storageValue > 0)
            {
                filter &= filterBuilder.Eq(x => x.Storage.Value, storageValue);
            }
        }

        if (!string.IsNullOrEmpty(request.PriceFrom))
        {
            decimal.TryParse(request.PriceFrom, out var priceFromValue);
            if(priceFromValue > 0)
            {
                filter &= filterBuilder.Gte(x => x.UnitPrice, priceFromValue);
            }
        }

        if (!string.IsNullOrEmpty(request.PriceTo))
        {
            decimal.TryParse(request.PriceTo, out var priceToValue);
            if (priceToValue > 0)
            {
                filter &= filterBuilder.Lte(x => x.UnitPrice, priceToValue);
            }
        }

        var sortBuilder = Builders<IPhone16Detail>.Sort;
        var sort = sortBuilder.Ascending(x => x.UnitPrice); // default sort

        if (!string.IsNullOrEmpty(request.PriceSort))
        {
            sort = request.PriceSort.ToLower() switch
            {
                "asc" => sortBuilder.Ascending(x => x.UnitPrice),
                "desc" => sortBuilder.Descending(x => x.UnitPrice),
                _ => throw new NotImplementedException(),
            };
        }

        return (filter, sort);
    }

    private PaginationResponse<ProductResponse> MapToResponse(List<IPhone16Detail> productItems, int totalRecords, int totalPages, GetProductsQuery request)
    {
        var queryParams = QueryParamBuilder.Build(request);

        var links = PaginationLinksBuilder.Build(basePath: "/api/v1/products",
                                                 queryParams: queryParams,
                                                 currentPage: request.Page ?? 1,
                                                 totalPages: totalPages);

        var productResponses = new List<ProductResponse>()
        {
            new ProductResponse
            {
                ProductId = "test"
            }
        };

        var response = new PaginationResponse<ProductResponse>
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Items = productResponses,
            Links = links
        };

        return response;
    }
}
