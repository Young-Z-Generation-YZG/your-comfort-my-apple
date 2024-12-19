

using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.Ordering.Domain.Orders.Events;
using Microsoft.FeatureManagement;

namespace YGZ.Ordering.Application.Orders.Events.Domains;

public class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IFeatureManager _featureManager;
    private readonly ILogger<OrderCreatedDomainEventHandler> _logger;

    public OrderCreatedDomainEventHandler(ILogger<OrderCreatedDomainEventHandler> logger, IPublishEndpoint publishEndpoint, IFeatureManager featureManager)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _featureManager = featureManager;
    }

    public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Order {OrderId} is successfully created", domainEvent.Order.Id);

        if(await _featureManager.IsEnabledAsync("OrderFullfilment"))
        {

        }

        throw new NotImplementedException();
    }
}
