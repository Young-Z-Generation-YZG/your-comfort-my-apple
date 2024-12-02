

using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.ServiceEvents.BasketEvents;

public record BasketCheckoutIntegrationEvent : IntergrationEvent
{
    public string UserId { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
}
