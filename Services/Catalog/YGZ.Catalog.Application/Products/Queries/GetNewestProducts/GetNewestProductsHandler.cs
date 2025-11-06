using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Products;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Product;

namespace YGZ.Catalog.Application.Products.Queries.GetRecommendationProducts;

public class GetNewestProductsHandler : IQueryHandler<GetNewestProductsQuery, PaginationResponse<NewestProductResponse>>
{
    private readonly ILogger<GetNewestProductsHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _modelRepository;

    public GetNewestProductsHandler(ILogger<GetNewestProductsHandler> logger, IMongoRepository<ProductModel, ModelId> modelRepository)
    {
        _logger = logger;
        _modelRepository = modelRepository;
    }

    public async Task<Result<PaginationResponse<NewestProductResponse>>> Handle(GetNewestProductsQuery request, CancellationToken cancellationToken)
    {
        // Default pagination: page 1, limit 5
        var page = 1;
        var limit = 5;

        // Sort by CreatedAt descending (newest first)
        var sort = Builders<ProductModel>.Sort.Descending("CreatedAt");

        // Filter by is_newest == true
        var filter = Builders<ProductModel>.Filter.Eq("is_newest", true);

        var result = await _modelRepository.GetAllAsync(
            _page: page,
            _limit: limit,
            filter: filter,
            sort: sort,
            cancellationToken: cancellationToken);

        var response = MapToResponse(result, page, limit);

        return response;
    }

    private static PaginationResponse<NewestProductResponse> MapToResponse((List<ProductModel> items, int totalRecords, int totalPages) result, int page, int limit)
    {
        var items = result.items.Select(model => new NewestProductResponse
        {
            Id = model.Id.Value!,
            Category = model.Category.ToResponse(),
            Name = model.Name,
            NormalizedModel = model.NormalizedModel,
            ProductClassification = model.ProductClassification,
            ModelItems = model.Models.Select(m => m.ToResponse()).ToList(),
            ColorItems = model.Colors.Select(c => c.ToResponse()).ToList(),
            StorageItems = model.Storages.Select(s => s.ToResponse()).ToList(),
            SkuPrices = model.Prices.Select(p => new IphoneSkuPriceListResponse
            {
                NormalizedModel = p.NormalizedModel,
                NormalizedColor = p.NormalizedColor,
                NormalizedStorage = p.NormalizedStorage,
                UnitPrice = p.UnitPrice
            }).ToList(),
            Description = model.Description,
            ShowcaseImages = model.ShowcaseImages.Select(img => img.ToResponse()).ToList(),
            IsNewest = model.IsNewest,
            Slug = model.Slug.Value!,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            UpdatedBy = model.UpdatedBy,
            IsDeleted = model.IsDeleted,
            DeletedAt = model.DeletedAt,
            DeletedBy = model.DeletedBy
        }).ToList();

        var links = PaginationLinksBuilder.Build(
            basePath: "",
            request: new { Page = page, Limit = limit },
            currentPage: page,
            totalPages: result.totalPages);

        return new PaginationResponse<NewestProductResponse>
        {
            TotalRecords = result.totalRecords,
            TotalPages = result.totalPages,
            PageSize = limit,
            CurrentPage = page,
            Links = links,
            Items = items
        };
    }
}
