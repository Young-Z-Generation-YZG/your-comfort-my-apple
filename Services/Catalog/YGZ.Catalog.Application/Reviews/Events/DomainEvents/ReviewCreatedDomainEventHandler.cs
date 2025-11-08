

using MassTransit;
using MediatR;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogServices;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.ProductModels;

namespace YGZ.Catalog.Application.Reviews.Events.DomainEvents;

public class ReviewCreatedDomainEventHandler : INotificationHandler<ReviewCreatedDomainEvent>
{
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;
    private readonly IPublishEndpoint _integrationEventSender;

    public ReviewCreatedDomainEventHandler(IMongoRepository<ProductModel, ModelId> productModelRepository, IPublishEndpoint integrationEventSender)
    {
        _productModelRepository = productModelRepository;
        _integrationEventSender = integrationEventSender;
    }

    public async Task Handle(ReviewCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var productModel = await _productModelRepository.GetByIdAsync(notification.Review.ModelId.Value!, cancellationToken);

        productModel.AddNewRating(notification.Review);

        await _productModelRepository.UpdateAsync(productModel.Id.Value!, productModel);

        // await _integrationEventSender.Publish(new ReviewCreatedIntegrationEvent
        // {
        //     OrderItemId = notification.Review.CustomerOrder.OrderItemId
        // }, cancellationToken);
    }
}
