using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Constants;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Notifications;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Notifications;
using YGZ.Ordering.Domain.Notifications.ValueObjects;

namespace YGZ.Ordering.Application.Notifications.Queries.GetNotifications;

public class GetNotificationsHandler : IQueryHandler<GetNotificationsQuery, PaginationResponse<NotificationResponse>>
{
    private readonly ILogger<GetNotificationsHandler> _logger;
    private readonly IGenericRepository<Notification, NotificationId> _repository;
    private readonly IUserHttpContext _userHttpContext;

    public GetNotificationsHandler(ILogger<GetNotificationsHandler> logger,
                                   IGenericRepository<Notification, NotificationId> repository,
                                   IUserHttpContext userHttpContext)
    {
        _logger = logger;
        _repository = repository;
        _userHttpContext = userHttpContext;
    }

    public async Task<Result<PaginationResponse<NotificationResponse>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var userId = _userHttpContext.GetUserId();
        var roles = _userHttpContext.GetUserRoles();
        var userIdValueObject = UserId.Of(userId);

        // Build filter expression based on user role
        var filterExpression = BuildExpression(request, userIdValueObject, roles);

        // Order by CreatedAt descending (newest first)
        Func<IQueryable<Notification>, IOrderedQueryable<Notification>> orderByCreatedAtDesc =
            query => query.OrderByDescending(n => n.CreatedAt);

        var result = await _repository.GetAllAsync(
            filterExpression: filterExpression,
            includeExpressions: null,
            orderBy: orderByCreatedAtDesc,
            page: request._page,
            limit: request._limit,
            cancellationToken: cancellationToken);

        var response = MapToResponse(result.items, result.totalRecords, result.totalPages, request);

        _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
            nameof(Handle), "Successfully retrieved notifications", new { userId, totalRecords = result.totalRecords, page = request._page, limit = request._limit });

        return response;
    }

    private static Expression<Func<Notification, bool>> BuildExpression(
        GetNotificationsQuery request,
        UserId userId,
        List<string> roles)
    {
        var filterExpression = ExpressionBuilder.New<Notification>();

        // Role-based filtering
        if (roles.Contains(AuthorizationConstants.Roles.ADMIN_SUPER))
        {
            // ADMIN_SUPER users see all notifications with type ORDER_CREATED
            filterExpression = filterExpression.And(n => n.ReceiverId == null);
        }
        else
        {
            // Regular users see only their own notifications
            filterExpression = filterExpression.And(n => n.ReceiverId == userId);
        }

        // Filter by types if provided
        if (request._types != null && request._types.Any())
        {
            var typeEnums = request._types
                .Select(type => EOrderNotificationType.FromName(type, false))
                .Where(type => type != null)
                .ToList();

            if (typeEnums.Any())
            {
                filterExpression = filterExpression.And(n => typeEnums.Contains(n.Type));
            }
        }

        // Filter by statuses if provided
        if (request._statuses != null && request._statuses.Any())
        {
            var statusEnums = request._statuses
                .Select(status => EOrderNotificationStatus.FromName(status, false))
                .Where(status => status != null)
                .ToList();

            if (statusEnums.Any())
            {
                filterExpression = filterExpression.And(n => statusEnums.Contains(n.Status));
            }
        }

        // Filter by IsRead if provided
        if (request._isRead.HasValue)
        {
            filterExpression = filterExpression.And(n => n.IsRead == request._isRead.Value);
        }

        // Exclude deleted notifications
        filterExpression = filterExpression.And(n => !n.IsDeleted);

        return filterExpression;
    }

    private PaginationResponse<NotificationResponse> MapToResponse(
        List<Notification> notifications,
        int totalRecords,
        int totalPages,
        GetNotificationsQuery request)
    {
        var queryParams = QueryParamBuilder.Build(request);

        var links = PaginationLinksBuilder.Build(
            basePath: "",
            queryParams: queryParams,
            currentPage: request._page ?? 1,
            totalPages: totalPages);

        var items = notifications.Select(n => n.ToResponse());

        var response = new PaginationResponse<NotificationResponse>
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            PageSize = request._limit ?? 10,
            CurrentPage = request._page ?? 1,
            Items = items,
            Links = links
        };

        return response;
    }
}
