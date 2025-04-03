
using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;

public record BasketCheckoutIntegrationEvent : IntegrationEvent
{
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
    public List<OrderLineIntegrationEvent> CartItems { get; set; } = new();
}

public record OrderLineIntegrationEvent(string ProductId,
                                        string ProductName,
                                        string ProductColorName,
                                        decimal ProductUnitPrice,
                                        string ProductNameTag,
                                        string ProductImage,
                                        string ProductSlug,
                                        int Quantity)
{ }

