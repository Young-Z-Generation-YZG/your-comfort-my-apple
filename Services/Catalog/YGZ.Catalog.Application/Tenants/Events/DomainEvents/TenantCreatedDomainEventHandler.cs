using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
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
    private readonly ILogger<TenantCreatedDomainEventHandler> _logger;
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;
    private readonly IMongoRepository<IphoneModel, ModelId> _iphoneModelRepository;
    private readonly IMongoRepository<IphoneSkuPrice, SkuPriceId> _iphoneSkuPriceRepository;
    private readonly IMongoRepository<Branch, BranchId> _branchRepository;

    public TenantCreatedDomainEventHandler(ILogger<TenantCreatedDomainEventHandler> logger,
                                           IMongoRepository<SKU, SkuId> skuRepository,
                                           IMongoRepository<IphoneModel, ModelId> iphoneModelRepository,
                                           IMongoRepository<Branch, BranchId> branchRepository,
                                           IMongoRepository<IphoneSkuPrice, SkuPriceId> iphoneSkuPriceRepository)
    {
        _logger = logger;
        _skuRepository = skuRepository;
        _iphoneModelRepository = iphoneModelRepository;
        _branchRepository = branchRepository;
        _iphoneSkuPriceRepository = iphoneSkuPriceRepository;
    }

    public async Task Handle(TenantCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _branchRepository.InsertOneAsync(notification.Branch);

        var iphoneModels = await _iphoneModelRepository.GetAllAsync();
        var iphoneSkuPrices = await _iphoneSkuPriceRepository.GetAllAsync();

        var skus = new List<SKU>();

        foreach (var model in iphoneModels)
        {
            foreach (var modelItem in model.Models)
            {
                foreach (var colorItem in model.Colors)
                {
                    foreach (var storageItem in model.Storages)
                    {
                        var iphoneSkuPrice = iphoneSkuPrices.FirstOrDefault(p => p.Model.NormalizedName == modelItem.NormalizedName && p.Storage.NormalizedName == storageItem.NormalizedName && p.Color.NormalizedName == colorItem.NormalizedName);

                        var sku = SKU.Create(skuId: notification.Tenant.TenantType == ETenantType.WAREHOUSE.Name ? SkuId.Of(iphoneSkuPrice?.Id.Value!) : SkuId.Create(),
                                             modelId: model.Id,
                                             tenantId: notification.Tenant.Id,
                                             branchId: notification.Branch.Id,
                                             skuCode: SkuCode.Create(productClassification: EProductClassification.IPHONE.Name,
                                                                     model: modelItem.Name,
                                                                     color: colorItem.Name,
                                                                     storage: storageItem.Name),
                                             productClassification: EProductClassification.IPHONE,
                                             model: modelItem,
                                             color: colorItem,
                                             storage: storageItem,
                                             unitPrice: iphoneSkuPrice.UnitPrice,
                                             availableInStock: 50);

                        skus.Add(sku);
                    }
                }
            }
        }

        await _skuRepository.InsertManyAsync(skus);
    }
}
