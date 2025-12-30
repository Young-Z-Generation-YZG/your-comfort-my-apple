using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Event;
using YGZ.Discount.Domain.Event.ValueObjects;
using ValueObjects = YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Application.Events.Commands.CreateEvent;

public class CreateEventHandler : ICommandHandler<CreateEventCommand, bool>
{
    private readonly ILogger<CreateEventHandler> _logger;
    private readonly IGenericRepository<Event, ValueObjects.EventId> _repository;
    
    public CreateEventHandler(IGenericRepository<Event, ValueObjects.EventId> repository, ILogger<CreateEventHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var entity = Event.Create(id: ValueObjects.EventId.Create(),
                                  title: request.Title,
                                  description: request.Description,
                                  startDate: request.StartDate,
                                  endDate: request.EndDate);

        var result = await _repository.AddAsync(entity, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_repository.AddAsync), "Failed to create event", new { title = request.Title, startDate = request.StartDate, endDate = request.EndDate, error = result.Error });

            return result.Error;
        }

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully created event", new { eventId = entity.Id.ToString(), title = request.Title, startDate = request.StartDate, endDate = request.EndDate });

        return result.Response;
    }
}
