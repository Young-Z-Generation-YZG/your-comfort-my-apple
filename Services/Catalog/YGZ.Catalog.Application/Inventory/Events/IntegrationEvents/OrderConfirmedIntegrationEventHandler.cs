using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;
using YGZ.Catalog.Application.Inventory.Commands.UpdateSkuCommand;

namespace YGZ.Catalog.Application.Inventory.Events.IntegrationEvents;

public class OrderConfirmedIntegrationEventHandler : IConsumer<OrderConfirmedIntegrationEvent>
{
    private readonly ILogger<OrderConfirmedIntegrationEventHandler> _logger;
    private readonly ISender _sender;

    public OrderConfirmedIntegrationEventHandler(ILogger<OrderConfirmedIntegrationEventHandler> logger,
                                                 ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<OrderConfirmedIntegrationEvent> context)
    {
        _logger.LogInformation("Integration envent handled: {IntegrationEvent}", context.Message.GetType().Name);


        await _sender.Send(new DeductQuantityCommand
        {
            Order = context.Message
        });
    }
}
