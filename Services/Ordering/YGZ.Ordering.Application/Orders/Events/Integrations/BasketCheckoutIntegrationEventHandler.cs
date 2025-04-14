

using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;
using YGZ.Ordering.Application.Orders.Commands.Common;
using YGZ.Ordering.Application.Orders.Commands.CreateOrder;
using YGZ.Ordering.Domain.Orders.ValueObjects;

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
        _logger.LogInformation("Integration envent handled: {IntegrationEvent}", context.Message.GetType().Name);

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

        var orderItems = context.OrderItems.Select(x => new OrderItemCommand
        {
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            ProductColorName = x.ProductColorName,
            ProductUnitPrice = x.ProductUnitPrice,
            ProductNameTag = x.ProductNameTag,
            ProductImage = x.ProductImage,
            ProductSlug = x.ProductSlug,
            Quantity = x.Quantity,
            Promotion = x.Promotion is not null
                ? new PromotionCommand
                {
                    PromotionIdOrCode = x.Promotion.PromotionIdOrCode,
                    PromotionEventType = x.Promotion.PromotionEventType,
                    PromotionTitle = x.Promotion.PromotionTitle,
                    PromotionDiscountType = x.Promotion.PromotionDiscountType,
                    PromotionDiscountValue = x.Promotion.PromotionDiscountValue,
                    PromotionDiscountUnitPrice = x.Promotion.PromotionDiscountUnitPrice,
                    PromotionAppliedProductCount = x.Promotion.PromotionAppliedProductCount,
                    PromotionFinalPrice = x.Promotion.PromotionFinalPrice
                }
                : null
        }).ToList();

        var command = new CreateOrderCommand(OrderId: context.OrderId,
                                             CustomerEmail: context.CustomerEmail,
                                             CustomerId: context.CustomerId,
                                             OrderItems: orderItems,
                                             ShippingAddress: shippingAddress,
                                             PaymentMethod: context.PaymentMethod,
                                             DiscountAmount: context.DiscountAmount,
                                             SubTotalAmount: context.SubTotalAmount,
                                             TotalAmount: context.TotalAmount);

        return command;
    }
}
