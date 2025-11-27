using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Notifications;

namespace YGZ.Ordering.Application.Notifications.Queries.GetNotifications;

public class GetNotificationsHandler : IQueryHandler<GetNotificationsQuery, PaginationResponse<NotificationResponse>>
{
    private readonly ILogger<GetNotificationsHandler> _logger;

    public GetNotificationsHandler(ILogger<GetNotificationsHandler> logger)
    {
        _logger = logger;
    }

    public Task<Result<PaginationResponse<NotificationResponse>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
