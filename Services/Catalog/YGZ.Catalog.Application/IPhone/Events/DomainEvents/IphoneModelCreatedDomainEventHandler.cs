
using MediatR;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Entities;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;

namespace YGZ.Catalog.Application.Iphone.Events.DomainEvents;

public class IphoneModelCreatedDomainEventHandler : INotificationHandler<IphoneModelCreatedDomainEvent>
{
    private readonly IMongoRepository<SKU, SKUId> _skuRepository;

    public IphoneModelCreatedDomainEventHandler(IMongoRepository<SKU, SKUId> mongoRepository)
    {
        _skuRepository = mongoRepository;
    }

    public async Task Handle(IphoneModelCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var skus = new List<SKU>();

        foreach (var model in notification.IphoneModel.Models)
        {
            foreach (var color in notification.IphoneModel.Colors)
            {
                foreach (var storage in notification.IphoneModel.Storages)
                {
                    var sku = SKU.Create(
                        skuCode: SKUCode.Create(EProductType.IPHONE.Name, model.Name, storage.Name, color.Name),
                        productType: EProductType.IPHONE,
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
    }
}
