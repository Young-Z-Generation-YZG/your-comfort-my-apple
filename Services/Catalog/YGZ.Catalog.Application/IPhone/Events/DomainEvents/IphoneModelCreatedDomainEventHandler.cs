
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.ProductModels;
using YGZ.Catalog.Domain.Tenants.Entities;
using YGZ.Catalog.Domain.Tenants.ValueObjects;

namespace YGZ.Catalog.Application.Iphone.Events.DomainEvents;

public class IphoneModelCreatedDomainEventHandler : INotificationHandler<IphoneModelCreatedDomainEvent>
{
    private readonly ILogger<IphoneModelCreatedDomainEventHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;
    private readonly IMongoRepository<SKU, SkuId> _skuRepository;
    private readonly IMongoRepository<IphoneSkuPrice, SkuPriceId> _iphoneSkuPriceRepository;

    public IphoneModelCreatedDomainEventHandler(ILogger<IphoneModelCreatedDomainEventHandler> logger,
                                                IMongoRepository<SKU, SkuId> mongoRepository,
                                                IMongoRepository<IphoneSkuPrice, SkuPriceId> iphoneSkuPriceRepository,
                                                IMongoRepository<ProductModel, ModelId> productModelRepository)
    {
        _logger = logger;
        _skuRepository = mongoRepository;
        _iphoneSkuPriceRepository = iphoneSkuPriceRepository;
        _productModelRepository = productModelRepository;
    }

    public async Task Handle(IphoneModelCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var skus = new List<SKU>();
        var iphoneSkuPrices = new List<IphoneSkuPrice>();

        foreach (var model in notification.IphoneModel.Models)
        {
            foreach (var color in notification.IphoneModel.Colors)
            {
                foreach (var storage in notification.IphoneModel.Storages)
                {
                    var price = notification.IphoneModel.Prices.FirstOrDefault(p => p.NormalizedModel == model.NormalizedName && p.NormalizedColor == color.NormalizedName && p.NormalizedStorage == storage.NormalizedName);

                    // var skuId = SkuId.Create();

                    // var sku = SKU.Create(skuId: skuId,
                    //                      modelId: notification.IphoneModel.Id,
                    //                      tenantId: TenantId.Create(),
                    //                      branchId: BranchId.Create(),
                    //                      skuCode: SkuCode.Create(EProductClassification.IPHONE.Name, model.Name, storage.Name, color.Name),
                    //                      productClassification: EProductClassification.IPHONE,
                    //                      model: model,
                    //                      color: color,
                    //                      storage: storage,
                    //                      unitPrice: price?.UnitPrice ?? 0,
                    //                      availableInStock: 50);

                    // skus.Add(sku);

                    var iphoneSkuPriceId = notification.IphoneModel.Prices.FirstOrDefault(p => p.NormalizedModel == model.NormalizedName && p.NormalizedColor == color.NormalizedName && p.NormalizedStorage == storage.NormalizedName)?.SkuId;

                    var iphoneSkuPrice = IphoneSkuPrice.Create(skuPriceId: SkuPriceId.Of(iphoneSkuPriceId),
                                                               modelId: notification.IphoneModel.Id,
                                                               model: model,
                                                               color: color,
                                                               storage: storage,
                                                               unitPrice: price?.UnitPrice ?? 0);

                    iphoneSkuPrices.Add(iphoneSkuPrice);
                }
            }
        }

        await _productModelRepository.InsertOneAsync(notification.ProductModel);

        // await _skuRepository.InsertManyAsync(skus);

        await _iphoneSkuPriceRepository.InsertManyAsync(iphoneSkuPrices);

    }
}
