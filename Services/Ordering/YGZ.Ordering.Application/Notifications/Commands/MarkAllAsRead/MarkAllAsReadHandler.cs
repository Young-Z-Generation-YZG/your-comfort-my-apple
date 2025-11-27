using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Notifications;
using YGZ.Ordering.Domain.Notifications.ValueObjects;

namespace YGZ.Ordering.Application.Notifications.Commands.MarkAllAsRead;

public class MarkAllAsReadHandler : ICommandHandler<MarkAllAsReadCommand, bool>
{
    private readonly ILogger<MarkAllAsReadHandler> _logger;
    private readonly IGenericRepository<Notification, NotificationId> _repository;
    private readonly IUserHttpContext _userHttpContext;

    public MarkAllAsReadHandler(ILogger<MarkAllAsReadHandler> logger,
                                 IGenericRepository<Notification, NotificationId> repository,
                                 IUserHttpContext userHttpContext)
    {
        _logger = logger;
        _repository = repository;
        _userHttpContext = userHttpContext;
    }

    public async Task<Result<bool>> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        var userId = _userHttpContext.GetUserId();
        var roles = _userHttpContext.GetUserRoles();
        var userIdValueObject = UserId.Of(userId);

        // Build filter expression based on user role (same as GetNotifications)
        var filterExpression = BuildFilterExpression(userIdValueObject, roles);

        // Get all unread notifications matching the filter
        var unreadFilter = ExpressionBuilder.New<Notification>()
            .And(filterExpression)
            .And(n => !n.IsRead)
            .And(n => !n.IsDeleted);

        var notifications = await _repository.GetAllAsync(
            filterExpression: unreadFilter,
            includeExpressions: null,
            orderBy: null,
            cancellationToken: cancellationToken);

        if (!notifications.Any())
        {
            return true;
        }

        // Mark all as read
        var now = DateTime.UtcNow;
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.UpdatedAt = now;
        }

        // Update all notifications in a single batch
        var result = await _repository.UpdateRangeAsync(notifications, cancellationToken);
        if (result.IsFailure)
        {
            _logger.LogError("Failed to update notifications for user {UserId}.", userId);
            return result.Error;
        }

        _logger.LogInformation("Marked {Count} notifications as read for user {UserId}.", notifications.Count, userId);
        return true;
    }

    private static Expression<Func<Notification, bool>> BuildFilterExpression(UserId userId, List<string> roles)
    {
        var filterExpression = ExpressionBuilder.New<Notification>();

        // Role-based filtering (same as GetNotifications)
        if (roles.Contains(AuthorizationConstants.Roles.ADMIN_SUPER))
        {
            // ADMIN_SUPER users see all notifications with ReceiverId == null
            filterExpression = filterExpression.And(n => n.ReceiverId == null);
        }
        else
        {
            // Regular users see only their own notifications
            filterExpression = filterExpression.And(n => n.ReceiverId == userId);
        }

        return filterExpression;
    }
}
