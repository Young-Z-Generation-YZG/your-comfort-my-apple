using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.IntegrationEvents.DiscountService;

public sealed record EventItemCreatedIntegrationEvent : IntegrationEvent
{
    public required string SkuId { get; init; }
    public required string EventId { get; init; }
    public required string EventItemId { get; init; }
    public required string TenantId { get; init; }
    public required string BranchId { get; init; }
    public required string EventName { get; init; }
    public required int ReservedQuantity { get; init; }
}
