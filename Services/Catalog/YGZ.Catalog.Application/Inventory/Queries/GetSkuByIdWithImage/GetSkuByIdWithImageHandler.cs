using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Contracts.Catalogs;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Errors;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Inventory.Queries.GetSkuByIdWithImage;

public class GetSkuByIdWithImageHandler : IQueryHandler<GetSkuByIdWithImageQuery, SkuWithImageResponse>
{
    private readonly ILogger<GetSkuByIdWithImageHandler> _logger;
    private readonly IMongoRepository<SKU, SkuId> _repository;
    private readonly IDistributedCache _distributedCache;

    public GetSkuByIdWithImageHandler(ILogger<GetSkuByIdWithImageHandler> logger, IMongoRepository<SKU, SkuId> repository, IDistributedCache distributedCache)
    {
        _logger = logger;
        _repository = repository;
        _distributedCache = distributedCache;
    }

    public async Task<Result<SkuWithImageResponse>> Handle(GetSkuByIdWithImageQuery request, CancellationToken cancellationToken)
    {
        var sku = await _repository.GetByIdAsync(request.SkuId, cancellationToken);

        if (sku is null)
        {
            _logger.LogError(":::[QueryHandler:{QueryHandler}][Result:Error][Method:{MethodName}]::: Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(GetSkuByIdWithImageHandler), nameof(_repository.GetByIdAsync), Errors.Inventory.SkuDoesNotExist.Message, request);

            return Errors.Inventory.SkuDoesNotExist;
        }

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
            ReservedForSkuRequests = sku.ReservedForSkuRequests.Select(x => x.ToResponse()).ToList(),
            State = sku.State,
            Slug = sku.Slug.Value,
            CreatedAt = sku.CreatedAt,
            UpdatedAt = sku.UpdatedAt,
            UpdatedBy = sku.UpdatedBy,
            DeletedAt = sku.DeletedAt,
            DeletedBy = sku.DeletedBy,
            IsDeleted = sku.IsDeleted
        };
    }
}
