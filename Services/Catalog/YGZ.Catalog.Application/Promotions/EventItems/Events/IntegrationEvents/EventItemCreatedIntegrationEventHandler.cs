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
        _logger.LogWarning(":::::[IntegrationEventHandler:{IntegrationEventHandler}]::::: Warning message: {Message}, Parameters: {@Parameters}",
            nameof(EventItemCreatedIntegrationEventHandler), "Processing event item created integration event", context.Message);

        var eventItemId = context.Message.EventItemId;
        var eventId = context.Message.EventId;
        var skuId = context.Message.SkuId;

        try
        {
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

            var result = await _sender.Send(command, context.CancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, ":::::[IntegrationEventHandler:{IntegrationEventHandler}][Exception]::::: Error message: {Message}, Parameters: {@Parameters}",
                nameof(EventItemCreatedIntegrationEventHandler), ex.Message, new { eventItemId, eventId, skuId });
            throw;
        }
    }
}
