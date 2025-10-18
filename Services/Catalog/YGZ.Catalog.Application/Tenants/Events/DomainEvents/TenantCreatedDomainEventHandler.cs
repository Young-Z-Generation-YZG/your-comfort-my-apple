using MediatR;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Enums;
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
    private readonly IMongoRepository<Branch, BranchId> _branchRepository;

    public TenantCreatedDomainEventHandler(IMongoRepository<SKU, SkuId> skuRepository, IMongoRepository<IphoneModel, ModelId> iphoneModelRepository, IMongoRepository<Branch, BranchId> branchRepository)
    {
        _skuRepository = skuRepository;
        _iphoneModelRepository = iphoneModelRepository;
        _branchRepository = branchRepository;
    }

    public async Task Handle(TenantCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _branchRepository.InsertOneAsync(notification.Branch);

        // Init SKUs for Iphone Models
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
                        var sku = SKU.Create(
                            modelId: model.Id,
                            tenantId: notification.Tenant.Id,
                            branchId: notification.Branch.Id,
                            skuCode: SkuCode.Create(EProductType.IPHONE.Name, modelItem.Name, storageItem.Name, colorItem.Name),
                            productType: EProductType.IPHONE,
                            model: modelItem,
                            color: colorItem,
                            storage: storageItem,
                            unitPrice: 0,
                            availableInStock: 0
                        );

                        skus.Add(sku);
                    }
                }
            }
        }

        await _skuRepository.InsertManyAsync(skus);


        // TODO: Implement SKU initialization logic for the new tenant

    }
}
