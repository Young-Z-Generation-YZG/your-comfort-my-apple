using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Discount.Application.Events.Extensions;
using YGZ.Discount.Domain.Abstractions.Data;
using YGZ.Discount.Domain.Event;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Application.Events.Commands.CreateEvent;

public class CreateEventHandler : ICommandHandler<CreateEventCommand, bool>
{
    private readonly IGenericRepository<Event, EventId> _repository;
    public CreateEventHandler(IGenericRepository<Event, EventId> repository)
    {
        _repository = repository;
    }

    public async Task<Result<bool>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var entity = request.ToEntity();

        var result = await _repository.AddAsync(entity, cancellationToken);

        return result.Response;
    }
}
