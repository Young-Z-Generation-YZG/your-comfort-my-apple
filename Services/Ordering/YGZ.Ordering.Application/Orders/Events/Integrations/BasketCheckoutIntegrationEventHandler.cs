

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
        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Consume), "Received basket checkout integration event", new { orderId = context.Message.OrderId, customerId = context.Message.CustomerId, tenantId = context.Message.TenantId });

        CreateOrderCommand command = MapToCreateOrderCommand(context.Message);

        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError(":::[Handler Error]::: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(_sender.Send), "Failed to create order from integration event", new { orderId = context.Message.OrderId, customerId = context.Message.CustomerId, error = result.Error });
        }
        else
        {
            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Consume), "Successfully processed basket checkout integration event", new { orderId = context.Message.OrderId, customerId = context.Message.CustomerId });
        }
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
            SkuId = item.SkuId,
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
                PromotionId = item.Promotion.PromotionId,
                PromotionType = item.Promotion.PromotionType,
                DiscountType = item.Promotion.DiscountType,
                DiscountValue = item.Promotion.DiscountValue,
            } : null
        }).ToList();

        var orderPromotion = !string.IsNullOrEmpty(context.Cart.PromotionId) ? new PromotionInOrderCommand
        {
            PromotionId = context.Cart.PromotionId,
            PromotionType = context.Cart.PromotionType ?? string.Empty,
            DiscountType = context.Cart.DiscountType ?? string.Empty,
            DiscountValue = context.Cart.DiscountValue ?? 0,
            DiscountAmount = context.Cart.DiscountAmount ?? 0
        } : null;

        var command = new CreateOrderCommand
        {
            OrderId = context.OrderId,
            CustomerId = context.CustomerId,
            TenantId = context.TenantId,
            BranchId = context.BranchId,
            CustomerPublicKey = context.CustomerPublicKey,
            Tx = context.Tx,
            CustomerEmail = context.CustomerEmail,
            ShippingAddress = shippingAddress,
            Promotion = orderPromotion,
            OrderItems = orderItems,
            PaymentMethod = context.PaymentMethod,
            TotalAmount = context.Cart.TotalAmount
        };

        return command;
    }
}
