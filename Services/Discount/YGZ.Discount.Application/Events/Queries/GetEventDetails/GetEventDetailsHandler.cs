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
            _logger.LogWarning("Event {EventId} was not found.", request.EventId);
            return Errors.Event.EventNotFound;
        }

        var eventItems = @event.EventItems?.Select(ei => ei.ToResponse()).ToList();

        return @event.ToResponse(eventItems);
    }
}
