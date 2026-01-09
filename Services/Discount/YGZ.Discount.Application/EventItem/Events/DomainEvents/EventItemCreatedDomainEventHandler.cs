using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.DiscountService;
using YGZ.Discount.Domain.Event.Events;

namespace YGZ.Discount.Application.EventItem.Events.DomainEvents;

public class EventItemCreatedDomainEventHandler : INotificationHandler<EventItemCreatedDomainEvent>
{
    private readonly IPublishEndpoint _integrationEventSender;
    private readonly IFeatureManager _featureManager;
    private readonly ILogger<EventItemCreatedDomainEventHandler> _logger;

    public EventItemCreatedDomainEventHandler(IFeatureManager featureManager,
                                              IPublishEndpoint integrationEventSender,
                                              ILogger<EventItemCreatedDomainEventHandler> logger)
    {
        _logger = logger;
        _featureManager = featureManager;
        _integrationEventSender = integrationEventSender;
    }

    public async Task Handle(EventItemCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning("::::[DomainEventHandler:{DomainEventHandler}]:::: Warning message: {Message}, Parameters: {@Parameters}",
            nameof(EventItemCreatedDomainEventHandler), "Processing event item created domain event", notification);

        var eventItemId = notification.EventItem.Id.Value.ToString();
        var eventId = notification.EventItem.EventId.Value.ToString() ?? "";

        var integrationEvent = new EventItemCreatedIntegrationEvent
        {
            SkuId = notification.EventItem.SkuId,
            EventId = eventId,
            EventItemId = eventItemId,
            TenantId = notification.EventItem.TenantId,
            BranchId = notification.EventItem.BranchId,
            EventName = "Black Friday 2025",
            ReservedQuantity = notification.EventItem.Stock,
        };

        try
        {
            _logger.LogWarning("####[DomainEventHandler:{DomainEventHandler}][IntegrationEvent:{IntegrationEvent}]#### Parameters: {@Parameters}",
                nameof(EventItemCreatedDomainEventHandler), nameof(EventItemCreatedIntegrationEvent), integrationEvent);
            await _integrationEventSender.Publish(integrationEvent, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "::::[DomainEventHandler:{DomainEventHandler}][Exception]:::: Error message: {Message}, Parameters: {@Parameters}",
                nameof(EventItemCreatedDomainEventHandler), ex.Message, new { eventItemId, eventId, skuId = notification.EventItem.SkuId });

            // throw to allow retry mechanisms or dead-letter queue handling
            throw;
        }
    }
}
