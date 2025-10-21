using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.DiscountService;
using YGZ.Catalog.Application.Promotions.EventItems.Commands.ReserveEventItemStock;

namespace YGZ.Catalog.Application.Promotions.EventItems.Events.IntegrationEvents;

public class EventItemCreatedIntegrationEventHandler : IConsumer<EventItemCreatedIntegrationEvent>
{
    private readonly ISender _sender;

    private readonly ILogger<EventItemCreatedIntegrationEventHandler> _logger;
    public EventItemCreatedIntegrationEventHandler(ISender sender,
                                                   ILogger<EventItemCreatedIntegrationEventHandler> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<EventItemCreatedIntegrationEvent> context)
    {
        _logger.LogInformation("Integration event received: {IntegrationEvent} for EventItemId: {EventItemId}", 
            nameof(EventItemCreatedIntegrationEvent), context.Message.EventItemId);

        var command = new ReserveEventItemStockCommand
        {
            SkuId = context.Message.SkuId,
            EventId = context.Message.EventId,
            EventItemId = context.Message.EventItemId,
            TenantId = context.Message.TenantId,
            BranchId = context.Message.BranchId,
            EventName = context.Message.EventName,
            ReservedQuantity = context.Message.ReservedQuantity
        };

        await _sender.Send(command);
        
        _logger.LogInformation("Successfully processed EventItemCreatedIntegrationEvent for EventItemId: {EventItemId}", 
            context.Message.EventItemId);
    }
}
