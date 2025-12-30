using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Core.Errors;
using Event = YGZ.Discount.Domain.Event.Event;
using ValueObjects = YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Application.Events.Queries.GetEventDetails;

public class GetEventDetailsHandler : IQueryHandler<GetEventDetailsQuery, EventResponse>
{
    private readonly ILogger<GetEventDetailsHandler> _logger;
    private readonly IGenericRepository<Event, ValueObjects.EventId> _repository;

    public GetEventDetailsHandler(IGenericRepository<Event, ValueObjects.EventId> repository, ILogger<GetEventDetailsHandler> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<EventResponse>> Handle(GetEventDetailsQuery request, CancellationToken cancellationToken)
    {
        var eventId = ValueObjects.EventId.Of(request.EventId);

        var @event = await _repository.DbSet
                                      .Where(e => e.Id == eventId)
                                      .Include(e => e.EventItems)
                                      .FirstOrDefaultAsync(cancellationToken);

        if (@event is null)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                "FirstOrDefaultAsync", "Event not found", new { eventId = request.EventId });

            return Errors.Event.EventNotFound;
        }

        var eventItems = @event.EventItems?.Select(ei => ei.ToResponse()).ToList();

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully retrieved event details", new { eventId = request.EventId, eventItemCount = eventItems?.Count ?? 0 });

        return @event.ToResponse(eventItems);
    }
}
