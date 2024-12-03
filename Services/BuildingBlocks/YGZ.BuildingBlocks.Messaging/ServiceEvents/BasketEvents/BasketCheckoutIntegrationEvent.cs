

using YGZ.BuildingBlocks.Messaging.Events;

namespace YGZ.BuildingBlocks.Messaging.ServiceEvents.BasketEvents;

public record BasketCheckoutIntegrationEvent : IntergrationEvent
{
    public string UserId { get; set; } = default!;
    public string ContactName { get; set; } = default!;
    public string ContactPhoneNumber { get; set; } = default!;
    public string ContactEmail { get; set; } = default!;
    public string AddressLine { get; set; } = default!;
    public string District { get; set; } = default!;
    public string Province { get; set; } = default!;
    public string Country { get; set; } = default!;
    public string PaymentType { get; set; } = default!;
    public required List<OrderLineIntegrationEvent> CartLines { get; set; }
}

public record OrderLineIntegrationEvent(string productItemId,
                                        string ProductModel,
                                        string ProductColor,
                                        int ProductStorage,
                                        string ProductSlug,
                                        int Quantity,
                                        decimal Price) { }

