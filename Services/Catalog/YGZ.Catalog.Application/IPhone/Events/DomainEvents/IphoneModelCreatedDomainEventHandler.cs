
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.ProductModels.ValueObjects;
using YGZ.Catalog.Domain.Products.ProductModels;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Iphone.Events.DomainEvents;

public class IphoneModelCreatedDomainEventHandler : INotificationHandler<IphoneModelCreatedDomainEvent>
{
    private readonly ILogger<IphoneModelCreatedDomainEventHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;

    public IphoneModelCreatedDomainEventHandler(ILogger<IphoneModelCreatedDomainEventHandler> logger,
                                                IMongoRepository<SKU, SkuId> mongoRepository,
                                                IMongoRepository<ProductModel, ModelId> productModelRepository)
    {
        _logger = logger;
        _skuRepository = mongoRepository;
        _productModelRepository = productModelRepository;
    }

    public async Task Handle(IphoneModelCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Initialize the skus list
        var skus = new List<SKU>();

        foreach (var model in notification.IphoneModel.Models)
        {
            foreach (var color in notification.IphoneModel.Colors)
            {
                foreach (var storage in notification.IphoneModel.Storages)
                {
                    var sku = SKU.Create(
                        modelId: notification.IphoneModel.Id,
                        tenantId: TenantId.Create(),
                        branchId: BranchId.Create(),
                        skuCode: SkuCode.Create(EProductClassification.IPHONE.Name, model.Name, storage.Name, color.Name),
                        productClassification: EProductClassification.IPHONE,
                        model: model,
                        color: color,
                        storage: storage,
                        unitPrice: 0,
                        availableInStock: 0
                    );

                    skus.Add(sku);
                }
            }
        }

        await _skuRepository.InsertManyAsync(skus);

        List<SkuPriceList> prices = notification.IphoneModel.Prices.Select(p => SkuPriceList.Create(p.NormalizedModel, p.NormalizedColor, p.NormalizedStorage, p.UnitPrice)).ToList();
    }
}
