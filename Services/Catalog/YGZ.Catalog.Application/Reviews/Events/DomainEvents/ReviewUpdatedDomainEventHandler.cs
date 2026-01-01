using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.Events;
using YGZ.Catalog.Domain.Products.ProductModels;

namespace YGZ.Catalog.Application.Reviews.Events.DomainEvents;

public class ReviewUpdatedDomainEventHandler : INotificationHandler<ReviewUpdatedDomainEvent>
{
    private readonly ILogger<ReviewUpdatedDomainEventHandler> _logger;
    private readonly IMongoRepository<ProductModel, ModelId> _productModelRepository;

    public ReviewUpdatedDomainEventHandler(
        ILogger<ReviewUpdatedDomainEventHandler> logger,
        IMongoRepository<ProductModel, ModelId> productModelRepository)
    {
        _logger = logger;
        _productModelRepository = productModelRepository;
    }

    public async Task Handle(ReviewUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var model = await _productModelRepository.GetByIdAsync(notification.OldReview.ModelId.Value!, cancellationToken, ignoreBaseFilter: true);

            if (model is null)
            {
                var warningParameters = new { ModelId = notification.OldReview.ModelId.Value, ReviewId = notification.OldReview.Id.Value };
                _logger.LogWarning(":::[Handler Warning]::: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                    nameof(Handle), "ProductModel not found when updating review", warningParameters);
                return;
            }

            model.UpdateRating(notification.OldReview, notification.NewReview);

            await _productModelRepository.UpdateAsync(model.Id.Value!, model, ignoreBaseFilter: true);

            var successParameters = new { ReviewId = notification.OldReview.Id.Value, ModelId = notification.OldReview.ModelId.Value };
            _logger.LogInformation(":::[Handler Information]::: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully updated review and product model rating", successParameters);
        }
        catch (Exception ex)
        {
            var parameters = new { ReviewId = notification.OldReview.Id.Value, ModelId = notification.OldReview.ModelId.Value };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), ex.Message, parameters);
            throw;
        }
    }
}
