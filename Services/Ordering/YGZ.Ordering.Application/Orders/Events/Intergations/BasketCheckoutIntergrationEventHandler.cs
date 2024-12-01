using YGZ.BuildingBlocks.Messaging.ServiceEvents.BasketEvents;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Ordering.Application.Orders.Commands.CreateOrder;
using static YGZ.Ordering.Domain.Core.Enums.Enums;
using YGZ.Ordering.Application.Common.Commands;

namespace YGZ.Ordering.Application.Orders.Events.Intergations;

public class BasketCheckoutIntergrationEventHandler : IConsumer<BasketCheckoutIntergrationEvent>
{
    private readonly ISender _mediator;
    private readonly ILogger<BasketCheckoutIntergrationEventHandler> _logger;

    public BasketCheckoutIntergrationEventHandler(ISender mediator, ILogger<BasketCheckoutIntergrationEventHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }


    public async Task Consume(ConsumeContext<BasketCheckoutIntergrationEvent> context)
    {
        _logger.LogInformation("Intergration Event Handled: {IntergrationEvent}", context.Message.GetType().Name);

        var cmd = MapToCreateOrderCommand(context.Message);

        await _mediator.Send(cmd);
    }

    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutIntergrationEvent message)
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
