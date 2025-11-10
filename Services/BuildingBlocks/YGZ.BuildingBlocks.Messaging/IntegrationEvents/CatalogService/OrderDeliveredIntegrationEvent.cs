using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;

public sealed record OrderDeliveredIntegrationEvent : IntegrationEvent
{
    public required string OrderId { get; init; }
    public required List<OrderItemIntegrationEvent> OrderItems { get; init; }
}
