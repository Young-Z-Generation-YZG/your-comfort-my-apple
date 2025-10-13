
using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;

public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
    public Guid OrderId { get; set; } = default!;
    public string CustomerId { get; set; } = default!;
    public string CustomerEmail { get; set; } = default!;
    public string PaymentMethod { get; set; } = default!;
    public string ContactName { get; set; } = default!;
    public string ContactPhoneNumber { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
    public string District { get; set; } = default!;
    public string Province { get; set; } = default!;
    public string Country { get; set; } = default!;
    public List<CheckoutItemIntegrationEvent> OrderItems { get; set; } = new();
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
    public required string PromotionIdOrCode { get; init; }
    public required string PromotionType { get; init; }
    public required string DiscountType { get; init; }
    public required decimal DiscountValue { get; init; }
    public required decimal DiscountAmount { get; init; }
    public required decimal FinalPrice { get; init; }

}