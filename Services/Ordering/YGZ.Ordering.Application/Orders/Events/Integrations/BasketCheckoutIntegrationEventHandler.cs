

using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;
using YGZ.Ordering.Application.Orders.Commands.CreateOrder;

namespace YGZ.Ordering.Application.Orders.Events.Integrations;

public class BasketCheckoutIntegrationEventHandler : IConsumer<BasketCheckoutIntegrationEvent>
{
    private readonly ISender _sender;

    private readonly ILogger<BasketCheckoutIntegrationEventHandler> _logger;
    public BasketCheckoutIntegrationEventHandler(ISender sender, ILogger<BasketCheckoutIntegrationEventHandler> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
    {
        _logger.LogInformation("Integration event handled: {IntegrationEvent}", context.Message.GetType().Name);

        CreateOrderCommand command = MapToCreateOrderCommand(context.Message);

        await _sender.Send(command);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutIntegrationEvent context)
    {
        var shippingAddress = new ShippingAddressCommand
        {
            ContactName = context.ContactName,
            ContactPhoneNumber = context.ContactPhoneNumber,
            AddressLine = context.AddressLine,
            District = context.District,
            Province = context.Province,
            Country = context.Country
        };

        var orderItems = context.Cart.OrderItems.Select(item => new OrderItemCommand
        {
            ModelId = item.ModelId,
            ProductName = item.ProductName,
            NormalizedModel = item.NormalizedModel,
            NormalizedColor = item.NormalizedColor,
            NormalizedStorage = item.NormalizedStorage,
            UnitPrice = item.UnitPrice,
            DisplayImageUrl = item.DisplayImageUrl,
            ModelSlug = item.ModelSlug,
            Quantity = item.Quantity,
            SubTotalAmount = item.SubTotalAmount,
            Promotion = item.Promotion != null ? new PromotionInItemCommand
            {
                PromotionIdOrCode = item.Promotion.PromotionIdOrCode,
                PromotionType = item.Promotion.PromotionType,
                DiscountType = item.Promotion.DiscountType,
                DiscountValue = item.Promotion.DiscountValue,
                DiscountAmount = item.Promotion.DiscountAmount,
                FinalPrice = item.Promotion.FinalPrice
            } : null
        }).ToList();

        var command = new CreateOrderCommand
        {
            OrderId = context.OrderId,
            CustomerId = context.CustomerId,
            CustomerEmail = context.CustomerEmail,
            ShippingAddress = shippingAddress,
            OrderItems = orderItems,
            PaymentMethod = context.PaymentMethod,
            TotalAmount = context.Cart.TotalAmount
        };

        return command;
    }
}
