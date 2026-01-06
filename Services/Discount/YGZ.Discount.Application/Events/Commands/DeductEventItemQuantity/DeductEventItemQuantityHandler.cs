using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Errors;
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
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                "FirstOrDefaultAsync", "Event not found", new { eventId = request.EventId });

            return Errors.Event.EventNotFound;
        }

        // Find event item in event
        var eventItemId = ValueObjects.EventItemId.Of(request.EventItemId);
        var eventItem = eventEntity.EventItems.FirstOrDefault(ei => ei.Id == eventItemId && !ei.IsDeleted);

        if (eventItem is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Event item not found in event", new { eventId = request.EventId, eventItemId = request.EventItemId });

            return Errors.EventItem.NotFound;
        }

        // Increase sold by deduct quantity
        try
        {
            eventItem.IncreaseSold(request.DeductQuantity);
        }
        catch (ArgumentException ex)
        {
            var parameters = new { eventId = request.EventId, eventItemId = request.EventItemId, deductQuantity = request.DeductQuantity };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(eventItem.IncreaseSold), ex.Message, parameters);

            return Errors.EventItem.InvalidQuantity;
        }
        catch (InvalidOperationException ex)
        {
            var parameters = new { eventId = request.EventId, eventItemId = request.EventItemId, deductQuantity = request.DeductQuantity, stock = eventItem.Stock, sold = eventItem.Sold };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(eventItem.IncreaseSold), ex.Message, parameters);
            return Errors.EventItem.InsufficientStock;
        }

        // Save changes
        var updateResult = await _repository.UpdateAsync(eventEntity, cancellationToken);

        if (updateResult.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.UpdateAsync), "Failed to update event after deducting quantity", new { eventId = request.EventId, eventItemId = request.EventItemId, error = updateResult.Error });

            return updateResult.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully increased sold quantity for event item", new { eventId = request.EventId, eventItemId = request.EventItemId, deductQuantity = request.DeductQuantity, newSold = eventItem.Sold });

        return true;
    }
}
