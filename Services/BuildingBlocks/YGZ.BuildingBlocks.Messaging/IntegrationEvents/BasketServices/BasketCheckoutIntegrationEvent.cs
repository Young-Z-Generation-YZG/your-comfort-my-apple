
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
    public decimal DiscountAmount { get; set; } = 0;
    public decimal SubTotalAmount { get; set; } = 0;
    public decimal TotalAmount { get; set; } = 0;
    public List<OrderItemIntegrationEvent> OrderItems { get; set; } = new();
}

public record OrderItemIntegrationEvent()
{
    public required string ProductId { get; set; }
    public required string ModelId { get; set; }
    public required string ProductName { get; set; }
    public required string ProductColorName { get; set; }
    public required decimal ProductUnitPrice { get; set; }
    public required string ProductNameTag { get; set; }
    public required string ProductImage { get; set; }
    public required string ProductSlug { get; set; }
    public required int Quantity { get; set; }
    public PromotionIntergrationEvent? Promotion { get; set; }
}

public record PromotionIntergrationEvent()
{
    public required string PromotionIdOrCode { get; set; }
    public required string PromotionEventType { get; set; }
    public required string PromotionTitle { get; set; }
    public required string PromotionDiscountType { get; set; }
    public required decimal PromotionDiscountValue { get; set; }
    public required decimal PromotionDiscountUnitPrice { get; set; }
    public required int PromotionAppliedProductCount { get; set; }
    public required decimal PromotionFinalPrice { get; set; }
}