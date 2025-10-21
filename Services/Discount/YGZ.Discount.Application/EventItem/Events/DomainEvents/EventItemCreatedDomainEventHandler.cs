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
        _logger.LogInformation("Handling EventItemCreatedDomainEvent for EventItemId: {EventItemId}", 
            notification.EventItem.Id.Value?.ToString() ?? "unknown");

        var integrationEvent = new EventItemCreatedIntegrationEvent
        {
            SkuId = notification.EventItem.SkuId,
            EventId = notification.EventItem.EventId?.Value?.ToString() ?? "",
            EventItemId = notification.EventItem.Id.Value?.ToString() ?? "",
            TenantId = notification.EventItem.TenantId,
            BranchId = notification.EventItem.BranchId,
            EventName = "",
            ReservedQuantity = notification.EventItem.Stock,
        };

        await _integrationEventSender.Publish(integrationEvent, cancellationToken);
        
        _logger.LogInformation("Successfully published EventItemCreatedIntegrationEvent for EventItemId: {EventItemId}", 
            integrationEvent.EventItemId);
    }
}
