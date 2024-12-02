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
        var paymentStaus = OrderStatus.PENDING.Name;
        var paymentType = PaymentType.MOMO.Name;

        var orderLines = new List<OrderLineCommand> {
            new OrderLineCommand(new Guid().ToString(), "iPhone 16", "iPhone 16", "pink", 256, "iphone-16", 1000, 1),
            new OrderLineCommand(new Guid().ToString(), "iPhone 16", "iPhone 16 Plus", "pink", 256, "iphone-16", 1000, 1)
        };

        return new CreateOrderCommand(
            message.UserId,
            null,
            null,
            paymentStaus,
            paymentType,
            orderLines
        );
    }
}
