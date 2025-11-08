
using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogServices;

public sealed record ReviewCreatedIntegrationEvent : IntegrationEvent
{
    public required string OrderItemId { get; init; }
}
