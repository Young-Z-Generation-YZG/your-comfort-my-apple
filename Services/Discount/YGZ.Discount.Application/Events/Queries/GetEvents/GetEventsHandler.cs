using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Discounts;
using YGZ.Discount.Domain.Abstractions.Data;
using Event = YGZ.Discount.Domain.Event.Event;
using ValueObjects = YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Application.Events.Queries.GetEvents;

public class GetEventsHandler : IQueryHandler<GetEventsQuery, List<EventResponse>>
{
    private readonly ILogger<GetEventsHandler> _logger;
    private readonly IGenericRepository<Event, ValueObjects.EventId> _repository;

    public GetEventsHandler(IGenericRepository<Event, ValueObjects.EventId> repository, ILogger<GetEventsHandler> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<List<EventResponse>>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _repository.DbSet
                                      .Include(e => e.EventItems)
                                      .ToListAsync(cancellationToken);

        if (events is null || events.Count == 0)
        {
            _logger.LogInformation("No events found.");
            return new List<EventResponse>();
        }

        var responses = events.Select(@event =>
        {
            var eventItems = @event.EventItems?.Select(ei => ei.ToResponse()).ToList();
            return @event.ToResponse(eventItems);
        }).ToList();

        return responses;
    }
}
