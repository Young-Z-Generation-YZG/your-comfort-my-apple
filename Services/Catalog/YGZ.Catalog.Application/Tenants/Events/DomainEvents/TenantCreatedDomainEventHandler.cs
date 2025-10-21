using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.Events;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Tenants.Events.DomainEvents;

public class TenantCreatedDomainEventHandler : INotificationHandler<TenantCreatedDomainEvent>
{
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;
    private readonly IMongoRepository<IphoneModel, ModelId> _iphoneModelRepository;
    private readonly IMongoRepository<IphoneSkuPrice, SkuPriceId> _iphoneSkuPriceRepository;
    private readonly IMongoRepository<Branch, BranchId> _branchRepository;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<TenantCreatedDomainEventHandler> _logger;

    public TenantCreatedDomainEventHandler(IMongoRepository<SKU, SkuId> skuRepository,
                                           IMongoRepository<IphoneModel, ModelId> iphoneModelRepository,
                                           IMongoRepository<Branch, BranchId> branchRepository,
                                           IMongoRepository<IphoneSkuPrice, SkuPriceId> iphoneSkuPriceRepository,
                                           IDistributedCache distributedCache,
                                           ILogger<TenantCreatedDomainEventHandler> logger)
    {
        _skuRepository = skuRepository;
        _iphoneModelRepository = iphoneModelRepository;
        _branchRepository = branchRepository;
        _distributedCache = distributedCache;
        _iphoneSkuPriceRepository = iphoneSkuPriceRepository;
        _logger = logger;
    }

    public async Task Handle(TenantCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _branchRepository.InsertOneAsync(notification.Branch);

        var iphoneModels = await _iphoneModelRepository.GetAllAsync();

        var skus = new List<SKU>();

        foreach (var model in iphoneModels)
        {
            foreach (var modelItem in model.Models)
            {
                foreach (var colorItem in model.Colors)
                {
                    foreach (var storageItem in model.Storages)
                    {
                        decimal unitPrice = 0;

                        var cachedKey = CacheKeyPrefixConstants.CatalogService.GetIphoneSkuPriceKey(
                            modelName: EIphoneModel.FromName(modelItem.NormalizedName),
                            storageName: EStorage.FromName(storageItem.NormalizedName),
                            colorName: EColor.FromName(colorItem.NormalizedName)
                        );
                        var cachedPrice = await _distributedCache.GetStringAsync(cachedKey, cancellationToken);

                        unitPrice = cachedPrice is not null ? decimal.Parse(cachedPrice) : 0;

                        if (cachedPrice is null)
                        {
                            var skuPrice = await _iphoneSkuPriceRepository.GetByFilterAsync(filter: Builders<IphoneSkuPrice>.Filter.Eq(x => x.UniqueQuery, cachedKey), cancellationToken: cancellationToken);

                            if (skuPrice is not null)
                            {
                                unitPrice = skuPrice.UnitPrice;
                            }
                        }

                        var sku = SKU.Create(
                            modelId: model.Id,
                            tenantId: notification.Tenant.Id,
                            branchId: notification.Branch.Id,
                            skuCode: SkuCode.Create(EProductClassification.IPHONE.Name, modelItem.Name, storageItem.Name, colorItem.Name),
                            productClassification: EProductClassification.IPHONE,
                            model: modelItem,
                            color: colorItem,
                            storage: storageItem,
                            unitPrice: unitPrice,
                            availableInStock: 0
                        );

                        skus.Add(sku);
                    }
                }
            }
        }

        await _skuRepository.InsertManyAsync(skus);
    }
}
