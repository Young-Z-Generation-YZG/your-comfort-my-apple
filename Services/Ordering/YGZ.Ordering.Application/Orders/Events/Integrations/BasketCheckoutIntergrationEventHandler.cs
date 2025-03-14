

using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.BasketService;

namespace YGZ.Ordering.Application.Orders.Events.Integrations;

public class BasketCheckoutIntergrationEventHandler : IConsumer<BasketCheckoutIntegrationEvent>
{
    private readonly ISender _sender;

    private readonly ILogger<BasketCheckoutIntergrationEventHandler> _logger;
    public BasketCheckoutIntergrationEventHandler(ISender sender, ILogger<BasketCheckoutIntergrationEventHandler> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
    {
        _logger.LogInformation("Integration envent handled: {IntegrationEvent}", context.Message.GetType().Name);

        return Task.CompletedTask;
    }
}
