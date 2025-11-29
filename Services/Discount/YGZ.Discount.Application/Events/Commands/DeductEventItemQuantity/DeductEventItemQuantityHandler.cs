using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Event;
using ValueObjects = YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Application.Events.Commands.DeductEventItemQuantity;

public class DeductEventItemQuantityHandler : ICommandHandler<DeductEventItemQuantityCommand, bool>
{
    private readonly ILogger<DeductEventItemQuantityHandler> _logger;
    private readonly IGenericRepository<Event, ValueObjects.EventId> _repository;

    public DeductEventItemQuantityHandler(ILogger<DeductEventItemQuantityHandler> logger, IGenericRepository<Event, ValueObjects.EventId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(DeductEventItemQuantityCommand request, CancellationToken cancellationToken)
    {
        // Get event by id with event items included
        var eventId = ValueObjects.EventId.Of(request.EventId);
        var eventEntity = await _repository.DbSet
            .Include(e => e.EventItems)
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        if (eventEntity is null)
        {
            _logger.LogError("Event with ID {EventId} not found.", request.EventId);
            return YGZ.Discount.Domain.Core.Errors.Errors.Event.EventNotFound;
        }

        // Find event item in event
        var eventItemId = ValueObjects.EventItemId.Of(request.EventItemId);
        var eventItem = eventEntity.EventItems.FirstOrDefault(ei => ei.Id == eventItemId && !ei.IsDeleted);

        if (eventItem is null)
        {
            _logger.LogError("Event item with ID {EventItemId} not found in event {EventId}.", request.EventItemId, request.EventId);
            return YGZ.Discount.Domain.Core.Errors.Errors.EventItem.EventItemNotFound;
        }

        // Increase sold by deduct quantity
        try
        {
            eventItem.IncreaseSold(request.DeductQuantity);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "Invalid quantity for event item {EventItemId}.", request.EventItemId);
            return YGZ.Discount.Domain.Core.Errors.Errors.EventItem.InvalidQuantity;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError(ex, "Cannot increase sold quantity for event item {EventItemId}.", request.EventItemId);
            return YGZ.Discount.Domain.Core.Errors.Errors.EventItem.InsufficientStock;
        }

        // Save changes
        await _repository.UpdateAsync(eventEntity, cancellationToken);

        _logger.LogInformation("Increased sold quantity for event item {EventItemId} by {Quantity}. New sold: {Sold}",
            request.EventItemId, request.DeductQuantity, eventItem.Sold);

        return true;
    }
}
