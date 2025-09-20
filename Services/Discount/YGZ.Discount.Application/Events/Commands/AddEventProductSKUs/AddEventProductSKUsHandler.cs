using Microsoft.EntityFrameworkCore; // Add this using directive for EF Core's Include extension method.
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Event;
using YGZ.Discount.Domain.Event.Entities;
using YGZ.Discount.Domain.Event.ValueObjects;
using _ValueObjects = YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Application.Events.Commands.AddEventProductSKUs;

public class AddEventProductSKUsHandler : ICommandHandler<AddEventProductSKUsCommand, bool>
{
    private readonly IGenericRepository<Event, _ValueObjects.EventId> _repository;
    private readonly ILogger<AddEventProductSKUsHandler> _logger;

    public AddEventProductSKUsHandler(IGenericRepository<Event, _ValueObjects.EventId> repository, ILogger<AddEventProductSKUsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(AddEventProductSKUsCommand request, CancellationToken cancellationToken)
    {
        if (!request.EventProductSKUs.Any())
        {
            _logger.LogError("No EventProductSKUs provided in the request.");

            return false;
        }

        // Group by EventId to process each event separately
        var groupedByEvent = request.EventProductSKUs.GroupBy(ep => ep.EventId);

        foreach (var eventGroup in groupedByEvent)
        {
            var eventId = _ValueObjects.EventId.Of(eventGroup.First().EventId);

            // Get the event with its existing EventProductSKUs
            var @event = await _repository.DbSet
                .Include(e => e.EventProductSKUs)
                .FirstOrDefaultAsync(e => e.Id == eventId && !e.IsDeleted, cancellationToken);

            if (@event is null)
            {
                _logger.LogError("Event not found with id: {EventId}", eventId);

                return false;
            }

            // Process each EventProductSKU for this event
            foreach (var eventProductSKUCommand in eventGroup)
            {
                // Check if SKU already exists for this event
                //var existingSKU = @event.GetEventProductSKUBySKUId(eventProductSKUCommand.SkuId);
                //if (existingSKU is not null)
                //{
                //    _logger.LogError("SKU already exists for event: {EventId}", eventGroup.Key);

                //    return false;
                //}

                // Create new EventProductSKU
                var eventProductSKU = EventProductSKU.Create(
                    id: EventProductSKUId.Create(),
                    eventId: eventId,
                    skuId: eventProductSKUCommand.SkuId,
                    modelId: eventProductSKUCommand.ModelId,
                    modelName: eventProductSKUCommand.ModelName,
                    storageName: eventProductSKUCommand.StorageName,
                    colorHaxCode: eventProductSKUCommand.ColorHaxCode,
                    productType: EProductType.FromName(eventProductSKUCommand.ProductType, false),
                    discountType: EDiscountType.FromName(eventProductSKUCommand.DiscountType, false),
                    imageUrl: eventProductSKUCommand.ImageUrl,
                    discountValue: eventProductSKUCommand.DiscountValue,
                    originalPrice: eventProductSKUCommand.OriginalPrice,
                    stock: eventProductSKUCommand.Stock
                );

                // Add to event
                @event.AddEventProductSKU(eventProductSKU);
            }

            // Update the event (this will also save the new EventProductSKUs due to EF Core change tracking)
            var updateResult = await _repository.UpdateAsync(@event, cancellationToken);

            if (!updateResult.IsSuccess)
            {
                _logger.LogError("Error updating event: {EventId}", eventId);

                return false;
            }
        }

        return true;
    }
}
