using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;
using YGZ.Catalog.Application.Inventory.Commands.UpdateSkuCommand;

namespace YGZ.Catalog.Application.Inventory.Events.IntegrationEvents;

public class OrderPaidIntegrationEventHandler : IConsumer<OrderPaidIntegrationEvent>
{
    private readonly ILogger<OrderPaidIntegrationEventHandler> _logger;
    private readonly ISender _sender;

    public OrderPaidIntegrationEventHandler(ILogger<OrderPaidIntegrationEventHandler> logger,
                                            ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<OrderPaidIntegrationEvent> context)
    {
        _logger.LogInformation("Integration event handled: {IntegrationEvent}", context.Message.GetType().Name);

        await _sender.Send(new DeductQuantityCommand
        {
            Order = context.Message.Order
        });
    }
}