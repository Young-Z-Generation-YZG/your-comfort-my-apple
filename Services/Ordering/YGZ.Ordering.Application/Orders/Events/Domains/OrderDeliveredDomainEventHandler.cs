using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;
using YGZ.Ordering.Domain.Orders.Events;

namespace YGZ.Ordering.Application.Orders.Events.Domains;

public class OrderDeliveredDomainEventHandler : INotificationHandler<OrderDeliveredDomainEvent>
{
    private readonly ILogger<OrderDeliveredDomainEventHandler> _logger;
    private readonly IPublishEndpoint _integrationEventSender;

    public OrderDeliveredDomainEventHandler(ILogger<OrderDeliveredDomainEventHandler> logger,
                                            IPublishEndpoint integrationEventSender)
    {
        _logger = logger;
        _integrationEventSender = integrationEventSender;
    }
    public async Task Handle(OrderDeliveredDomainEvent notification, CancellationToken cancellationToken)
    {
        await _integrationEventSender.Publish(new OrderDeliveredIntegrationEvent
        {
            OrderId = notification.Order.Id.Value.ToString(),
            OrderItems = notification.Order.OrderItems.Select(x => new OrderItemIntegrationEvent
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
