
using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;

public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public required Guid OrderId { get; init; }
    public required string CustomerId { get; init; }
    public required string CustomerEmail { get; init; }
    public string? CustomerPublicKey { get; init; }
    public string? Tx { get; init; }
    public required string ContactName { get; init; }
    public required string ContactPhoneNumber { get; init; }
    public required string AddressLine { get; init; }
    public required string District { get; init; }
    public required string Province { get; init; }
    public required string Country { get; init; }
    public required string PaymentMethod { get; init; }
    public required CartCommand Cart { get; init; }
}

public record CartCommand
{
    public required List<CheckoutItemIntegrationEvent> OrderItems { get; init; }
    public string? PromotionId { get; init; }
    public string? PromotionType { get; init; }
    public string? DiscountType { get; init; }
    public decimal? DiscountValue { get; init; }
    public decimal? DiscountAmount { get; init; }
    public required decimal TotalAmount { get; init; }

}

public record CheckoutItemIntegrationEvent
{
    public required string ModelId { get; init; }
    public required string ProductName { get; init; }
    public required string NormalizedModel { get; init; }
    public required string NormalizedColor { get; init; }
    public required string NormalizedStorage { get; init; }
    public required decimal UnitPrice { get; init; }
    public required string DisplayImageUrl { get; init; }
    public required string ModelSlug { get; init; }
    public required int Quantity { get; init; }
    public required decimal SubTotalAmount { get; init; }
    public PromotionIntegrationEvent? Promotion { get; init; }
}

public record PromotionIntegrationEvent
{
    public required string PromotionId { get; init; }
    public required string PromotionType { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal DiscountAmount { get; init; }
    public required decimal FinalPrice { get; init; }

}