using YGZ.BuildingBlocks.Shared.Notifications.Models;

namespace YGZ.BuildingBlocks.Shared.Abstractions.Notifications;

public interface IOrderNotificationClient
{
    Task OrderStatusUpdated(OrderNotificationModel order);
}
