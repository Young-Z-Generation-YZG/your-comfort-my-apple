using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.ProductModels;

namespace YGZ.Catalog.Application.Reviews.Events.DomainEvents;

public class ReviewDeletedDomainEventHandler : INotificationHandler<ReviewDeletedDomainEvent>
{
    private readonly ILogger<ReviewDeletedDomainEventHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;

    public ReviewDeletedDomainEventHandler(ILogger<ReviewDeletedDomainEventHandler> logger, IMongoRepository<ProductModel, ModelId> productModelRepository)
    {
        _logger = logger;
        _productModelRepository = productModelRepository;
    }

    public async Task Handle(ReviewDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var model = await _productModelRepository.GetByIdAsync(notification.Review.ModelId.Value!, cancellationToken);

        if (model is null)
        {
            _logger.LogWarning("ProductModel with Id {ModelId} not found when deleting review {ReviewId}", notification.Review.ModelId.Value, notification.Review.Id.Value);
            return;
        }

        model.DeleteRating(notification.Review);

        await _productModelRepository.UpdateAsync(model.Id.Value!, model);
    }
}
