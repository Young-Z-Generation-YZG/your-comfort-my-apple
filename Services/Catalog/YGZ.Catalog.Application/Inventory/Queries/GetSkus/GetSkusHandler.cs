using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Inventory.Queries.GetWarehouses;

public class GetSkusHandler : IQueryHandler<GetSkusQuery, PaginationResponse<SkuWithImageResponse>>
{
    private readonly ILogger<GetSkusHandler> _logger;
    private readonly IMongoRepository<SKU, SkuId> _repository;
    private readonly IDistributedCache _distributedCache;
    private readonly IUserHttpContext _userHttpContext;

    public GetSkusHandler(IMongoRepository<SKU, SkuId> repository, ILogger<GetSkusHandler> logger, IDistributedCache distributedCache, IUserHttpContext userHttpContext)
    {
        _repository = repository;
        _logger = logger;
        _distributedCache = distributedCache;
        _userHttpContext = userHttpContext;
    }

    public async Task<Result<PaginationResponse<SkuWithImageResponse>>> Handle(GetSkusQuery request, CancellationToken cancellationToken)
    {
        var (filter, sort) = GetFilterDefinition(request);

        var result = await _repository.GetAllAsync(
            _page: request._page,
            _limit: request._limit,
            filter: filter,
            sort: sort,
            cancellationToken: cancellationToken);

        var response = await MapToResponse(result, request);

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully retrieved SKUs", new { page = request._page ?? 1, limit = request._limit ?? 10, totalRecords = result.totalRecords, totalPages = result.totalPages, tenantId = request._tenantId });

        return response;
    }

    private static (FilterDefinition<SKU> filter, SortDefinition<SKU>? sort) GetFilterDefinition(GetSkusQuery request)
    {
        var filterBuilder = Builders<SKU>.Filter;
        var filter = filterBuilder.Empty;

        if (request._tenantId is not null)
        {
            filter &= filterBuilder.Eq("tenant_id", new ObjectId(request._tenantId));
        }

        if (request._colors is not null && request._colors.Any())
        {
            var productColors = request._colors.ToList();

            filter &= filterBuilder.In("color.normalized_name", productColors);
        }

        if (request._storages is not null && request._storages.Any())
        {
            var productStorages = request._storages.ToList();

            filter &= filterBuilder.In("storage.normalized_name", productStorages);
        }

        if (request._models is not null && request._models.Any())
        {
            var productModels = request._models.ToList();
            filter &= filterBuilder.In("model.normalized_name", productModels);
        }

        // Dynamic filters for stock
        if (request._stock.HasValue && !string.IsNullOrEmpty(request._stockOperator))
        {
            filter &= request._stockOperator switch
            {
                ">=" => filterBuilder.Gte(x => x.AvailableInStock, request._stock.Value),
                ">" => filterBuilder.Gt(x => x.AvailableInStock, request._stock.Value),
                "<=" => filterBuilder.Lte(x => x.AvailableInStock, request._stock.Value),
                "<" => filterBuilder.Lt(x => x.AvailableInStock, request._stock.Value),
                "==" => filterBuilder.Eq(x => x.AvailableInStock, request._stock.Value),
                "!=" => filterBuilder.Ne(x => x.AvailableInStock, request._stock.Value),
                "in" => HandleInOperator(filterBuilder, x => x.AvailableInStock, request._stock.Value), // TODO: Enhance for range support
                _ => filterBuilder.Empty
            };
        }

        // Dynamic filters for sold
        if (request._sold.HasValue && !string.IsNullOrEmpty(request._soldOperator))
        {
            filter &= request._soldOperator switch
            {
                ">=" => filterBuilder.Gte(x => x.TotalSold, request._sold.Value),
                ">" => filterBuilder.Gt(x => x.TotalSold, request._sold.Value),
                "<=" => filterBuilder.Lte(x => x.TotalSold, request._sold.Value),
                "<" => filterBuilder.Lt(x => x.TotalSold, request._sold.Value),
                "==" => filterBuilder.Eq(x => x.TotalSold, request._sold.Value),
                "!=" => filterBuilder.Ne(x => x.TotalSold, request._sold.Value),
                "in" => HandleInOperator(filterBuilder, x => x.TotalSold, request._sold.Value), // TODO: Enhance for range support
                _ => filterBuilder.Empty
            };
        }

        return (filter, null);
    }

    private static FilterDefinition<SKU> HandleInOperator(
        FilterDefinitionBuilder<SKU> filterBuilder,
        System.Linq.Expressions.Expression<Func<SKU, int>> fieldExpression,
        int value)
    {
        // For "in" operator with range support, you would parse a range string like "30-50"
        // For now, this is a placeholder - you may need to adjust based on how ranges are passed
        // Example: if value is passed as a string "30-50", parse it here and use Gte/Lte
        // For simplicity, treating single value as exact match for now
        // You can enhance this to parse range strings like "30-50" from a separate parameter
        return filterBuilder.Eq(fieldExpression, value);
    }

    private async Task<PaginationResponse<SkuWithImageResponse>> MapToResponse((List<SKU> items, int totalRecords, int totalPages) result, GetSkusQuery request)
    {
        var items = await Task.WhenAll(result.items.Select(async sku =>
        {
            var displayImageUrl = await _distributedCache.GetStringAsync(CacheKeyPrefixConstants.CatalogService.GetDisplayImageUrlKey(sku.ModelId.Value ?? "", EColor.FromName(sku.Color.NormalizedName)), CancellationToken.None);

            return new SkuWithImageResponse
            {
                Id = sku.Id.Value ?? "",
                Code = sku.SkuCode.Value,
                ModelId = sku.ModelId.Value ?? "",
                TenantId = sku.TenantId.Value ?? "",
                BranchId = sku.BranchId.Value ?? "",
                ProductClassification = sku.ProductClassification,
                Model = sku.Model.ToResponse(),
                Color = sku.Color.ToResponse(),
                Storage = sku.Storage.ToResponse(),
                DisplayImageUrl = displayImageUrl ?? "",
                UnitPrice = sku.UnitPrice,
                AvailableInStock = sku.AvailableInStock,
                TotalSold = sku.TotalSold,
                ReservedForEvent = sku.ReservedForEvent?.ToResponse(),
                State = sku.State,
                Slug = sku.Slug.Value,
                CreatedAt = sku.CreatedAt,
                UpdatedAt = sku.UpdatedAt,
                DeletedAt = sku.DeletedAt,
                DeletedBy = sku.DeletedBy,
                IsDeleted = sku.IsDeleted
            };
        }));

        var links = PaginationLinksBuilder.Build(
            basePath: "",
            request: request,
            currentPage: request._page,
            totalPages: result.totalPages);

        return new PaginationResponse<SkuWithImageResponse>
        {
            TotalRecords = result.totalRecords,
            TotalPages = result.totalPages,
            PageSize = request._limit ?? 10,
            CurrentPage = request._page ?? 1,
            Links = links,
            Items = items
        };
    }
}
