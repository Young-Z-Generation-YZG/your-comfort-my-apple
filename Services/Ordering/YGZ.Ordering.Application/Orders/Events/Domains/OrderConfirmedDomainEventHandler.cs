using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;
using YGZ.Ordering.Domain.Orders.Events;

namespace YGZ.Ordering.Application.Orders.Events.Domains;

public class OrderConfirmedDomainEventHandler : INotificationHandler<OrderConfirmedDomainEvent>
{
    private readonly ILogger<OrderConfirmedDomainEventHandler> _logger;
    private readonly IPublishEndpoint _integrationEventSender;

    public OrderConfirmedDomainEventHandler(ILogger<OrderConfirmedDomainEventHandler> logger,
                                            IPublishEndpoint integrationEventSender)
    {
        _logger = logger;
        _integrationEventSender = integrationEventSender;
    }

    public async Task Handle(OrderConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventSender.Publish(new OrderConfirmedIntegrationEvent
        {
            OrderId = notification.order.Id.Value.ToString(),
            OrderItems = notification.order.OrderItems.Select(x => new OrderItemIntegrationEvent
            {
                ModelId = x.ModelId,
                SkuId = x.SkuId ?? string.Empty,
                NormalizedModel = x.ModelName,
                NormalizedColor = x.ColorName,
                NormalizedStorage = x.StorageName,
                Quantity = x.Quantity,
                PromotionId = x.PromotionId,
                PromotionType = x.PromotionType
            }).ToList()
        }, cancellationToken);
    }
}
