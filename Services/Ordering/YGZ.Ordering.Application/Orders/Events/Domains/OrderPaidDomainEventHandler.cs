using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.Events;
using YGZ.Ordering.Domain.Notifications;
using YGZ.Ordering.Domain.Notifications.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Events.Domains;

public class OrderPaidDomainEventHandler : INotificationHandler<OrderPaidDomainEvent>
{
    private readonly ILogger<OrderPaidDomainEventHandler> _logger;
    private readonly IPublishEndpoint _integrationEventSender;
    private readonly IGenericRepository<Notification, NotificationId> _notificationRepository;

    public OrderPaidDomainEventHandler(ILogger<OrderPaidDomainEventHandler> logger,
                                       IPublishEndpoint integrationEventSender,
                                       IGenericRepository<Notification, NotificationId> notificationRepository)
    {
        _logger = logger;
        _integrationEventSender = integrationEventSender;
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(OrderPaidDomainEvent notification, CancellationToken cancellationToken)
    {
        var orderItems = notification.Order.OrderItems.Select(x => new OrderItemIntegrationEvent
        {
            ModelId = x.ModelId,
            SkuId = x.SkuId ?? string.Empty,
            NormalizedModel = x.ModelName,
            NormalizedColor = x.ColorName,
            NormalizedStorage = x.StorageName,
            Quantity = x.Quantity,
            PromotionId = x.PromotionId,
            PromotionType = x.PromotionType
        }).ToList();


        await _integrationEventSender.Publish(new OrderPaidIntegrationEvent
        {
            Order = new OrderIntegrationEvent
            {
                OrderId = notification.Order.Id.Value.ToString(),
                OrderItems = orderItems
            }
        }, cancellationToken);

        await CreateNotificationsAsync(notification, cancellationToken);
    }

    private async Task CreateNotificationsAsync(OrderPaidDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var order = domainEvent.Order;
        var orderCode = order.Code.Value;
        var customerId = order.CustomerId.Value.ToString();

        var adminNotification = Notification.Create(
            title: "Order paid",
            content: $"Order {orderCode} has been paid and is ready for fulfillment.",
            type: EOrderNotificationType.ORDER_STATUS_UPDATED.Name,
            status: EOrderNotificationStatus.PAID.Name,
            receiverId: null!,
            senderId: null);

        var userNotification = Notification.Create(
            title: "Payment received",
            content: $"We received your payment for order {orderCode}.",
            type: EOrderNotificationType.ORDER_STATUS_UPDATED.Name,
            status: EOrderNotificationStatus.PAID.Name,
            receiverId: customerId,
            senderId: null);

        var adminResult = await _notificationRepository.AddAsync(adminNotification, cancellationToken);
        if (adminResult.IsFailure)
        {
            _logger.LogWarning("Failed to create admin payment notification for order {OrderId}", order.Id.Value);
        }

        var userResult = await _notificationRepository.AddAsync(userNotification, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("Failed to create user payment notification for order {OrderId}", order.Id.Value);
        }
    }
}