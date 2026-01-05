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
        var eventItemId = notification.EventItem.Id.Value.ToString();
        var eventId = notification.EventItem.EventId.Value.ToString() ?? "";

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Processing event item created domain event", new { eventItemId, eventId, skuId = notification.EventItem.SkuId });

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

        await _integrationEventSender.Publish(integrationEvent, cancellationToken);

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully published event item created integration event", new { eventItemId, eventId, skuId = notification.EventItem.SkuId });
    }
}
