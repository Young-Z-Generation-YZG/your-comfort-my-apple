using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Messaging.IntegrationEvents.CatalogService;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Abstractions.Emails;
using YGZ.Ordering.Application.Emails;
using YGZ.Ordering.Application.Emails.Models;
using YGZ.Ordering.Domain.Orders.Events;
using YGZ.Ordering.Domain.Notifications;
using YGZ.Ordering.Domain.Notifications.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Events.Domains;

public class OrderPaidDomainEventHandler : INotificationHandler<OrderPaidDomainEvent>
{
    private readonly ILogger<OrderPaidDomainEventHandler> _logger;
    private readonly IPublishEndpoint _integrationEventSender;
    private readonly IGenericRepository<Notification, NotificationId> _notificationRepository;
    private readonly IEmailService _emailService;

    public OrderPaidDomainEventHandler(ILogger<OrderPaidDomainEventHandler> logger,
                                       IPublishEndpoint integrationEventSender,
                                       IGenericRepository<Notification, NotificationId> notificationRepository,
                                       IEmailService emailService)
    {
        _logger = logger;
        _integrationEventSender = integrationEventSender;
        _notificationRepository = notificationRepository;
        _emailService = emailService;
    }

    public async Task Handle(OrderPaidDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Processing order paid domain event", new { orderId = notification.Order.Id.Value, orderCode = notification.Order.Code.Value, customerId = notification.Order.CustomerId.Value });

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
        
        SendEmailAsync(notification, cancellationToken);
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
            _logger.LogWarning("::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                nameof(_notificationRepository.AddAsync), "Failed to create admin payment notification", new { orderId = order.Id.Value, orderCode, error = adminResult.Error });
        }

        var userResult = await _notificationRepository.AddAsync(userNotification, cancellationToken);
        if (userResult.IsFailure)
        {
            _logger.LogWarning("::[Operation Warning]:: Method: {MethodName}, Warning message: {WarningMessage}, Parameters: {@Parameters}",
                nameof(_notificationRepository.AddAsync), "Failed to create user payment notification", new { orderId = order.Id.Value, orderCode, customerId, error = userResult.Error });
        }
    }

    private async Task SendEmailAsync(OrderPaidDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        try
        {
            var order = domainEvent.Order;
            var customerEmail = order.ShippingAddress.ContactEmail;
            var customerName = order.ShippingAddress.ContactName;

            var emailModel = new OrderPaidEmailModel
            {
                CustomerName = customerName,
                OrderCode = order.Code.Value,
                OrderId = order.Id.Value.ToString(),
                CreatedAt = order.CreatedAt,
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(item => new OrderItemEmailModel
                {
                    ModelName = item.ModelName,
                    ColorName = item.ColorName,
                    StorageName = item.StorageName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    SubTotalAmount = item.SubTotalAmount,
                    DiscountAmount = item.DiscountAmount,
                    TotalAmount = item.TotalAmount,
                    DisplayImageUrl = item.DisplayImageUrl
                }).ToList(),
                ShippingAddress = new ShippingAddressEmailModel
                {
                    ContactName = order.ShippingAddress.ContactName,
                    ContactEmail = order.ShippingAddress.ContactEmail,
                    ContactPhoneNumber = order.ShippingAddress.ContactPhoneNumber,
                    AddressLine = order.ShippingAddress.AddressLine,
                    District = order.ShippingAddress.District,
                    Province = order.ShippingAddress.Province,
                    Country = order.ShippingAddress.Country
                }
            };

            var emailCommand = new EmailCommand(
                ReceiverEmail: customerEmail,
                Subject: $"Payment Received - Order {order.Code.Value}",
                ViewName: "OrderPaid",
                Model: emailModel
            );

            await _emailService.SendEmailAsync(emailCommand);
            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(SendEmailAsync), "Successfully sent order payment email", new { orderId = order.Id.Value, orderCode = order.Code.Value, customerEmail });
        }
        catch (Exception ex)
        {
            var parameters = new { orderId = domainEvent.Order.Id.Value, orderCode = domainEvent.Order.Code.Value, customerEmail = domainEvent.Order.ShippingAddress.ContactEmail };
            _logger.LogError(ex, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(SendEmailAsync), ex.Message, parameters);
            // Don't throw - email failure shouldn't break the order payment flow
        }
    }
}