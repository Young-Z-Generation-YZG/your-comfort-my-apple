using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;

namespace YGZ.Catalog.Application.ProductModels.Queries.GetProductsByCategory;

public class GetProductModelsByCategoryHandler : IQueryHandler<GetProductModelsByCategoryQuery, PaginationResponse<ProductModelResponse>>
{
    private readonly ILogger<GetProductModelsByCategoryHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _mongoRepository;

    public GetProductModelsByCategoryHandler(ILogger<GetProductModelsByCategoryHandler> logger, IMongoRepository<ProductModel, ModelId> mongoRepository)
    {
        _logger = logger;
        _mongoRepository = mongoRepository;
    }

    public async Task<Result<PaginationResponse<ProductModelResponse>>> Handle(GetProductModelsByCategoryQuery request, CancellationToken cancellationToken)
    {
        var (filter, sort) = GetFilterDefinition(request);

        var page = request.Page ?? 1;
        var limit = request.Limit ?? 10;

        var allModels = await _mongoRepository.GetAllAsync(page, limit, filter, sort, cancellationToken);

        PaginationResponse<ProductModelResponse> response = MapToResponse(allModels, request);

        return response;
    }

    private PaginationResponse<ProductModelResponse> MapToResponse((List<ProductModel> items, int totalRecords, int totalPages) allModels, GetProductModelsByCategoryQuery request)
    {
        var items = allModels.items.Select(model => model.ToResponse()).ToList();

        return new PaginationResponse<ProductModelResponse>
        {
            TotalRecords = allModels.totalRecords,
            TotalPages = allModels.totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Links = new PaginationLinks("", "", "", ""),
            Items = items
        };
    }

    private (FilterDefinition<ProductModel> filterBuilder, SortDefinition<ProductModel> sort) GetFilterDefinition(GetProductModelsByCategoryQuery request)
    {
        var filterBuilder = Builders<ProductModel>.Filter;
        var filter = filterBuilder.Empty;

        // Filter by category slug
        if (!string.IsNullOrWhiteSpace(request.CategorySlug))
        {
            filter &= filterBuilder.Eq(x => x.Category.Slug, Slug.Of(request.CategorySlug));
        }

        // Filter by colors
        if (request.Colors is not null && request.Colors.Any())
        {
            var productColors = request.Colors.ToList();
            filter &= filterBuilder.AnyIn(x => x.Colors.Select(c => c.NormalizedName), productColors);
        }

        // Filter by storages
        if (request.Storages is not null && request.Storages.Any())
        {
            var productStorages = request.Storages.ToList();
            filter &= filterBuilder.AnyIn(x => x.Storages.Select(s => s.NormalizedName), productStorages);
        }

        // Filter by models
        if (request.Models is not null && request.Models.Any())
        {
            var productModels = request.Models.Select(x => x.ToUpper()).ToList();
            filter &= filterBuilder.AnyIn(x => x.Models.Select(m => m.NormalizedName), productModels);
        }

        // Filter by minimum price
        if (!string.IsNullOrEmpty(request.MinPrice))
        {
            decimal.TryParse(request.MinPrice, out var priceFromValue);
            if (priceFromValue > 0)
            {
                filter &= filterBuilder.ElemMatch(x => x.Prices, p => p.UnitPrice >= priceFromValue);
            }
        }

        // Filter by maximum price
        if (!string.IsNullOrEmpty(request.MaxPrice))
        {
            decimal.TryParse(request.MaxPrice, out var priceToValue);
            if (priceToValue > 0)
            {
                filter &= filterBuilder.ElemMatch(x => x.Prices, p => p.UnitPrice <= priceToValue);
            }
        }

        // Build sort definition
        var sortBuilder = Builders<ProductModel>.Sort;
        var sort = sortBuilder.Ascending("prices.unit_price"); // default sort by minimum price

        if (!string.IsNullOrEmpty(request.PriceSort))
        {
            sort = request.PriceSort.ToUpper() switch
            {
                "ASC" => sortBuilder.Ascending("prices.unit_price"),
                "DESC" => sortBuilder.Descending("prices.unit_price"),
                _ => sortBuilder.Ascending("prices.unit_price"),
            };
        }

        return (filter, sort);
    }
}
