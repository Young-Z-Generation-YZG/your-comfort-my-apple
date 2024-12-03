using YGZ.BuildingBlocks.Messaging.ServiceEvents.BasketEvents;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Ordering.Application.Orders.Commands.CreateOrder;
using static YGZ.Ordering.Domain.Core.Enums.Enums;
using YGZ.Ordering.Application.Common.Commands;

namespace YGZ.Ordering.Application.Orders.Events.Integrations;

public class BasketCheckoutIntegrationEventHandler : IConsumer<BasketCheckoutIntegrationEvent>
{
    private readonly ISender _mediator;
    private readonly ILogger<BasketCheckoutIntegrationEventHandler> _logger;

    public BasketCheckoutIntegrationEventHandler(ISender mediator, ILogger<BasketCheckoutIntegrationEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
    {
        _logger.LogInformation("Intergration Event Handled: {IntergrationEvent}", context.Message.GetType().Name);

        var cmd = MapToCreateOrderCommand(context.Message);

        await _mediator.Send(cmd);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutIntegrationEvent message)
    {
        var paymentStatus = OrderStatus.PENDING.Name;
        var paymentType = PaymentType.FromName(message.PaymentType);

        var address = new AddressCommand
            (
            message.ContactName,
            message.ContactPhoneNumber,
            message.ContactEmail,
            message.AddressLine,
            message.District,
            message.Province,
            message.Country
            );

        var orderLines = message.CartLines.Select(o => new OrderLineCommand(
            o.productItemId,
            o.ProductModel,
            o.ProductColor,
            o.ProductStorage,
            o.ProductSlug,
            o.Price,
            o.Quantity,
            o.Quantity * o.Price
        )).ToList();

        return new CreateOrderCommand(
            message.UserId,
            address,
            address,
            paymentStatus,
            paymentType,
            orderLines
        );
    }
}
