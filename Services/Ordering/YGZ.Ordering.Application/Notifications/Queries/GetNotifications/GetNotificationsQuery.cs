using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Notifications;

namespace YGZ.Ordering.Application.Notifications.Queries.GetNotifications;

public sealed record GetNotificationsQuery : IQuery<PaginationResponse<NotificationResponse>>
{
    public int? _page { get; init; }
    public int? _limit { get; init; }
}
