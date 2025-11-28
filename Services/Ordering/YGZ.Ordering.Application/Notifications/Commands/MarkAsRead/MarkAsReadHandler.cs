using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Notifications;
using YGZ.Ordering.Domain.Notifications.ValueObjects;

namespace YGZ.Ordering.Application.Notifications.Commands.MarkAsRead;

public class MarkAsReadHandler : ICommandHandler<MarkAsReadCommand, bool>
{
    private readonly ILogger<MarkAsReadHandler> _logger;
    private readonly IGenericRepository<Notification, NotificationId> _repository;
    private readonly IUserHttpContext _userHttpContext;

    public MarkAsReadHandler(ILogger<MarkAsReadHandler> logger,
                              IGenericRepository<Notification, NotificationId> repository,
                              IUserHttpContext userHttpContext)
    {
        _logger = logger;
        _repository = repository;
        _userHttpContext = userHttpContext;
    }

    public async Task<Result<bool>> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        var notificationId = NotificationId.Of(request.NotificationId);
        var notification = await _repository.GetByIdAsync(notificationId, cancellationToken: cancellationToken);

        if (notification is null)
        {
            return Errors.Notification.NotFound;
        }

        // Mark as read
        notification.MarkAsRead();

        var result = await _repository.UpdateAsync(notification, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        return true;
    }
}
