namespace YGZ.BuildingBlocks.Shared.Notifications.Models;

public class OrderNotificationModel
{
    public Guid OrderId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
