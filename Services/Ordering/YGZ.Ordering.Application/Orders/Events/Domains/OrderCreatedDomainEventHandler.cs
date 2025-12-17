

using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.OrderingServices;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Notifications;
using YGZ.Ordering.Domain.Notifications.ValueObjects;
using YGZ.Ordering.Domain.Orders.Events;

namespace YGZ.Ordering.Application.Orders.Events.Domains;

public class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
{
    private readonly ILogger<OrderCreatedDomainEventHandler> _logger;
    private readonly IFeatureManager _featureManager;
    private readonly IPublishEndpoint _integrationEventSender;
    private readonly IGenericRepository<Notification, NotificationId> _notificationRepository;

    public OrderCreatedDomainEventHandler(ILogger<OrderCreatedDomainEventHandler> logger,
                                          IFeatureManager featureManager,
                                          IPublishEndpoint integrationEventSender,
                                          IGenericRepository<Notification, NotificationId> notificationRepository)
    {
        _logger = logger;
        _featureManager = featureManager;
        _integrationEventSender = integrationEventSender;
        _notificationRepository = notificationRepository;
    }

    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().Name);

        if (await _featureManager.IsEnabledAsync("OrderFulfillment"))
        {
            var orderCreatedIntegrationEvent = new OrderFullfilmentIntegrationEvent();
            //await _integrationEventSender.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }

        await CreateNotificationsAsync(notification, cancellationToken);
    }

    private async Task CreateNotificationsAsync(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var order = domainEvent.Order;
        var orderCode = order.Code.Value;
        var customerId = order.CustomerId.Value.ToString();

        var adminNotification = Notification.Create(
            title: "New order created",
            content: $"Order {orderCode} has been created and awaits processing.",
            type: EOrderNotificationType.ORDER_CREATED.Name,
            status: EOrderNotificationStatus.PENDING.Name,
            receiverId: null!,
            senderId: null);

        var adminResult = await _notificationRepository.AddAsync(adminNotification, cancellationToken);
        if (adminResult.IsFailure)
        {
            _logger.LogWarning("Failed to create admin notification for order {OrderId}", order.Id.Value);
        }
    }
}