
using MediatR;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.ProductModels;

namespace YGZ.Catalog.Application.Reviews.Events.DomainEvents;

public class ReviewUpdatedDomainEventHandler : INotificationHandler<ReviewUpdatedDomainEvent>
{
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;

    public ReviewUpdatedDomainEventHandler(IMongoRepository<ProductModel, ModelId> productModelRepository)
    {
        _productModelRepository = productModelRepository;
    }

    public async Task Handle(ReviewUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var model = await _productModelRepository.GetByIdAsync(notification.OldReview.ModelId.Value!, cancellationToken);

        if (model is null)
        {
            return;
        }

        model.UpdateRating(notification.OldReview, notification.NewReview);

        await _productModelRepository.UpdateAsync(model.Id.Value!, model);
    }
}
