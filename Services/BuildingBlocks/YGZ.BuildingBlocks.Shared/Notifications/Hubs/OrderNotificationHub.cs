using Microsoft.AspNetCore.SignalR;
using YGZ.BuildingBlocks.Shared.Abstractions.Notifications;
using Microsoft.AspNetCore.Authorization;

namespace YGZ.BuildingBlocks.Shared.Notifications.Hubs;

[Authorize]
public sealed class OrderNotificationHub : Hub<IOrderNotificationClient>
{

}
