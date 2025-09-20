using YGZ.Discount.Application.Events.Commands.CreateEvent;
using YGZ.Discount.Domain.Event;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Application.Events.Extensions;

public static class MappingExtension
{
    public static Event ToEntity(this CreateEventCommand request)
    {
        return Event.Create(id: EventId.Create(),
                            title: request.Title,
                            description: request.Description,
                            startDate: request.StartDate,
                            endDate: request.EndDate);
    }
}
