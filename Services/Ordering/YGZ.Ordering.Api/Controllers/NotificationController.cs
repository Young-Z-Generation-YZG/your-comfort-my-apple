using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Ordering.Api.Contracts;
using YGZ.Ordering.Application.Notifications.Commands.CreateNotification;
using YGZ.Ordering.Application.Notifications.Commands.MarkAllAsRead;
using YGZ.Ordering.Application.Notifications.Commands.MarkAsRead;
using YGZ.Ordering.Application.Notifications.Queries.GetNotifications;

namespace YGZ.Ordering.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/orders/notifications")]
[OpenApiTag("notification", Description = "Notification check.")]
[AllowAnonymous]
public class NotificationController : ApiController
{
    private readonly ILogger<NotificationController> _logger;
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public NotificationController(ILogger<NotificationController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateNotificationCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetNotifications([FromQuery] GetNotificationsRequest request, CancellationToken cancellationToken)
    {
        var query = _mapper.Map<GetNotificationsQuery>(request);

        var result = await _sender.Send(query, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPut("{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead([FromRoute] string notificationId, CancellationToken cancellationToken)
    {
        var command = new MarkAsReadCommand
        {
            NotificationId = notificationId
        };

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }

    [HttpPut("read-all")]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken)
    {
        var command = new MarkAllAsReadCommand();

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
