

using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.OrderingServices;
using YGZ.Ordering.Domain.Orders.Events;

namespace YGZ.Ordering.Application.Orders.Events.Domains;

public class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
{
    private readonly ILogger<OrderCreatedDomainEventHandler> _logger;
    private readonly IFeatureManager _featureManager;
    private readonly IPublishEndpoint _integrationEventSender;

    public OrderCreatedDomainEventHandler(ILogger<OrderCreatedDomainEventHandler> logger, IFeatureManager featureManager, IPublishEndpoint integrationEventSender)
    {
        _logger = logger;
        _featureManager = featureManager;
        _integrationEventSender = integrationEventSender;
    }

    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

        if(await _featureManager.IsEnabledAsync("OrderFulfillment"))
        {
            var orderCreatedIntegrationEvent  = new OrderFullfilmentIntegrationEvent();
            //await _integrationEventSender.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }
    }
}
