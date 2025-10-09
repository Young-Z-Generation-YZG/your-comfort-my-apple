using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Event;
using ValueObjects = YGZ.Discount.Domain.Event.ValueObjects;
using EventItemEntity = YGZ.Discount.Domain.Event.Entities.EventItem;

namespace YGZ.Discount.Application.Events.Commands.AddEventItem;

public class AddEventItemsHandler : ICommandHandler<AddEventItemsCommand, bool>
{
    private readonly IGenericRepository<Event, ValueObjects.EventId> _repository;
    private readonly ILogger<AddEventItemsHandler> _logger;

    public AddEventItemsHandler(IGenericRepository<Event, ValueObjects.EventId> repository, ILogger<AddEventItemsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(AddEventItemsCommand request, CancellationToken cancellationToken)
    {
        if (!request.EventItems.Any())
        {
            _logger.LogError("No EventItems provided in the request.");

            return false;
        }

        // Get the event
        var eventId = ValueObjects.EventId.Of(request.EventId);

        var eventEntity = await _repository.GetByIdAsync(eventId, cancellationToken);

        if (eventEntity is null)
        {
            _logger.LogError("Event with ID {EventId} not found.", request.EventId);

            return false;
        }

        // Create and add event itemsz
        foreach (var itemCmd in request.EventItems)
        {
            var productType = EProductType.FromName(itemCmd.ProductType, false);
            var discountType = EDiscountType.FromName(itemCmd.DiscountType, false);

            var eventItem = EventItemEntity.Create(
                eventId: eventId,
                modelName: itemCmd.Model.Name,
                normalizedModel: itemCmd.Model.NormalizedName,
                colorName: itemCmd.Color.Name,
                normalizedColor: itemCmd.Color.NormalizedName,
                colorHaxCode: itemCmd.Color.HexCode,
                storageName: itemCmd.Storage.Name,
                normalizedStorage: itemCmd.Storage.NormalizedName,
                productType: productType,
                discountType: discountType,
                imageUrl: itemCmd.DisplayImageUrl,
                discountValue: itemCmd.DiscountValue,
                originalPrice: itemCmd.OriginalPrice,
                stock: itemCmd.Stock
            );

            eventEntity.AddEventItem(eventItem);
        }

        // Save changes
        await _repository.UpdateAsync(eventEntity, cancellationToken);

        _logger.LogInformation("Added {Count} event items to event {EventId}", request.EventItems.Count, request.EventId);

        return true;
    }
}
