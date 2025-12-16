using YGZ.BuildingBlocks.Shared.Abstractions.Data;
using YGZ.BuildingBlocks.Shared.Contracts.Notifications;
using YGZ.BuildingBlocks.Shared.Domain.Core.Primitives;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Domain.Notifications.ValueObjects;

namespace YGZ.Ordering.Domain.Notifications;

public class Notification : AggregateRoot<NotificationId>, IAuditable, ISoftDelete
{
    public Notification(NotificationId id) : base(id) { }

    public UserId? SenderId { get; init; }
    public UserId? ReceiverId { get; init; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required EOrderNotificationType Type { get; init; }
    public required EOrderNotificationStatus Status { get; set; }
    public bool IsRead { get; set; } = false;
    public string? Link { get; init; }  
    public bool IsSystem { get; set; } = true;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; } = null;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; } = null;
    public string? DeletedBy { get; set; } = null;


    public static Notification Create(string title,
                                      string content,
                                      string type,
                                      string status,
                                      string? receiverId,
                                      string? senderId = null,
                                      string? link = null,
                                      bool isRead = false,
                                      bool isSystem = true)
    {
        EOrderNotificationType.TryFromName(type, out var typeEnum);
        if (typeEnum is null)
        {
            throw new ArgumentException("Invalid notification type", nameof(type));
        }

        EOrderNotificationStatus.TryFromName(status, out var statusEnum);
        if (statusEnum is null)
        {
            throw new ArgumentException("Invalid notification status", nameof(status));
        }  

        return new Notification(NotificationId.Create()) {
            Title = title,
            Content = content,
            Type = typeEnum,
            Status = statusEnum,
            ReceiverId = receiverId is not null ? UserId.Of(receiverId) : null,
            SenderId = senderId is not null ? UserId.Of(senderId) : null,
            Link = link,
            IsRead = isRead,
            IsSystem = isSystem
        };
    }



    /// <summary>
    /// Creates a system notification for order status updates to customers
    /// </summary>
    // public static Notification CreateOrderStatusNotification(
    //     NotificationId notificationId,
    //     UserId customerId,
    //     string orderCode,
    //     EOrderStatus orderStatus,
    //     string? orderLink = null)
    // {
    //     var (title, content) = GetOrderStatusNotificationContent(orderCode, orderStatus);

    //     return new Notification(notificationId)
    //     {
    //         Title = title,
    //         Content = content,
    //         Type = EOrderNotificationType.ORDER_STATUS_UPDATE,
    //         Status = EOrderNotificationStatus.PENDING,
    //         ReceiverId = customerId,
    //         SenderId = null, // System notification
    //         Link = orderLink,
    //         IsRead = false,
    //         IsSystem = true
    //     };
    // }

    /// <summary>
    /// Creates a system notification for new order creation to ADMIN_SUPER users
    /// </summary>
    // public static Notification CreateNewOrderNotification(
    //     NotificationId notificationId,
    //     UserId adminSuperUserId,
    //     string orderCode,
    //     string customerEmail,
    //     decimal totalAmount,
    //     string? orderLink = null)
    // {
    //     return new Notification(notificationId)
    //     {
    //         Title = "New Order Created",
    //         Content = $"A new order {orderCode} has been created by customer {customerEmail} with total amount of {totalAmount:C}.",
    //         Type = EOrderNotificationType.NEW_ORDER_CREATED,
    //         Status = EOrderNotificationStatus.PENDING,
    //         ReceiverId = adminSuperUserId,
    //         SenderId = null, // System notification
    //         Link = orderLink,
    //         IsRead = false,
    //         IsSystem = true
    //     };
    // }

    /// <summary>
    /// Marks the notification as read
    /// </summary>
    public void MarkAsRead()
    {
        IsRead = true;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the notification status
    /// </summary>
    public void UpdateStatus(EOrderNotificationStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Maps the notification to a response DTO
    /// </summary>
    public NotificationResponse ToResponse()
    {
        return new NotificationResponse
        {
            Id = Id.Value.ToString(),
            SenderId = SenderId?.Value.ToString(),
            ReceiverId = ReceiverId?.Value.ToString(),
            Title = Title,
            Content = Content,
            Type = Type.Name,
            Status = Status.Name,
            IsRead = IsRead,
            Link = Link,
            IsSystem = IsSystem,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
            UpdatedBy = UpdatedBy,
            IsDeleted = IsDeleted,
            DeletedAt = DeletedAt,
            DeletedBy = DeletedBy
        };
    }

    private static (string title, string content) GetOrderStatusNotificationContent(string orderCode, EOrderStatus orderStatus)
    {
        return orderStatus.Name switch
        {
            nameof(EOrderStatus.CONFIRMED) => (
                "Order Confirmed",
                $"Your order {orderCode} has been confirmed and is being prepared."
            ),
            nameof(EOrderStatus.PREPARING) => (
                "Order Being Prepared",
                $"Your order {orderCode} is currently being prepared."
            ),
            nameof(EOrderStatus.DELIVERING) => (
                "Order Out for Delivery",
                $"Your order {orderCode} is out for delivery and will arrive soon."
            ),
            nameof(EOrderStatus.DELIVERED) => (
                "Order Delivered",
                $"Your order {orderCode} has been delivered successfully. Thank you for your purchase!"
            ),
            nameof(EOrderStatus.CANCELLED) => (
                "Order Cancelled",
                $"Your order {orderCode} has been cancelled."
            ),
            nameof(EOrderStatus.PAID) => (
                "Payment Received",
                $"Payment for your order {orderCode} has been received successfully."
            ),
            _ => (
                "Order Status Updated",
                $"Your order {orderCode} status has been updated to {orderStatus.Name}."
            )
        };
    }
}
