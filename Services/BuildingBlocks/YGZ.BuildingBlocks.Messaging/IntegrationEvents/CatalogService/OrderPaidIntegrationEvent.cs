using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;

public sealed record OrderPaidIntegrationEvent : IntegrationEvent
{
    public required OrderIntegrationEvent Order { get; init; }
}