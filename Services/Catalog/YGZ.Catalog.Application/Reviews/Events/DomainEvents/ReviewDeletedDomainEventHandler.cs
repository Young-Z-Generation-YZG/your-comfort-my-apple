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
        try
        {
            var model = await _productModelRepository.GetByIdAsync(notification.Review.ModelId.Value!, cancellationToken, ignoreBaseFilter: true);

            if (model is null)
            {
                var warningParameters = new { ModelId = notification.Review.ModelId.Value, ReviewId = notification.Review.Id.Value };
                _logger.LogWarning(":::[Handler Warning]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "ProductModel not found when deleting review", warningParameters);
                return;
            }

            model.DeleteRating(notification.Review);

            await _productModelRepository.UpdateAsync(model.Id.Value!, model, ignoreBaseFilter: true);

            var successParameters = new { ReviewId = notification.Review.Id.Value, ModelId = notification.Review.ModelId.Value };
            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully deleted review and updated product model rating", successParameters);
        }
        catch (Exception ex)
        {
            var parameters = new { ReviewId = notification.Review.Id.Value, ModelId = notification.Review.ModelId.Value };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            throw;
        }
    }
}
