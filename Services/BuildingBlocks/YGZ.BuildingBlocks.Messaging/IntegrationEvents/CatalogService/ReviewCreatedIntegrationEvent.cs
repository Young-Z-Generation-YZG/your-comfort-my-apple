
using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogServices;

public sealed record ReviewCreatedIntegrationEvent : IntegrationEvent
{
    public required string ReviewId { get; set; }
    public required string OrderItemId { get; set; }
    public required string CustomerId { get; set; }
    public required string ReviewContent { get; set; }
    public required int ReviewStar { get; set; }
    public required bool IsReviewed { get; set; }
}
