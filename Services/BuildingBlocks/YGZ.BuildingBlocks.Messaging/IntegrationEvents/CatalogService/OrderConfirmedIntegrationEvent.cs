using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;

public sealed record OrderConfirmedIntegrationEvent : IntegrationEvent
{
    public required string OrderId { get; init; }
    public required List<OrderItemIntegrationEvent> OrderItems { get; init; }
}

public sealed record OrderItemIntegrationEvent
{
    public required string SkuId { get; init; }
    public required string ModelId { get; init; }
    public required string NormalizedModel { get; init; }
    public required string NormalizedColor { get; init; }
    public required string NormalizedStorage { get; init; }
    public required int Quantity { get; init; }
    public string? PromotionId { get; init; }
    public string? PromotionType { get; init; }
}