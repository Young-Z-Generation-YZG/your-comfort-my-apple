using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Notifications;
using YGZ.Ordering.Domain.Notifications.ValueObjects;

namespace YGZ.Ordering.Application.Notifications.Commands.CreateNotification;

public class CreateNotificationHandler : ICommandHandler<CreateNotificationCommand, bool>
{
    private readonly ILogger<CreateNotificationHandler> _logger;
    private readonly IGenericRepository<Notification, NotificationId> _repository;
    private readonly IUserHttpContext _userHttpContext;

    public CreateNotificationHandler(ILogger<CreateNotificationHandler> logger,
                                     IGenericRepository<Notification, NotificationId> repository,
                                     IUserHttpContext userHttpContext)
    {
        _logger = logger;
        _repository = repository;
        _userHttpContext = userHttpContext;
    }

    public async Task<Result<bool>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var userId = _userHttpContext.GetUserId();

        Notification? notification = null;

        EOrderNotificationType.TryFromName(request.Type, out var typeEnum);
        if (typeEnum is null)
        {
            throw new ArgumentException("Invalid notification type", nameof(request.Type));
        }

        /*
        if ORDER_CREATED, this notification will be sent to ADMIN_SUPER users
        => receiverId will be null
        else, this notification will be sent to the user who has the role USER
        => receiverId will be the userId of the user who has the role USER
        ***/
        if (typeEnum == EOrderNotificationType.ORDER_CREATED)
        {
            notification = Notification.Create(title: request.Title,
                                               content: request.Content,
                                               type: request.Type,
                                               status: request.Status,
                                               receiverId: null,
                                               senderId: null,
                                               link: request.Link);
        }
        else
        {
            notification = Notification.Create(title: request.Title,
                                               content: request.Content,
                                               type: request.Type,
                                               status: request.Status,
                                               receiverId: userId,
                                               senderId: null,
                                               link: request.Link);
        }


        var result = await _repository.AddAsync(notification, cancellationToken);

        if (result.IsFailure)
        {
            return false;
        }

        return true;
    }
}
