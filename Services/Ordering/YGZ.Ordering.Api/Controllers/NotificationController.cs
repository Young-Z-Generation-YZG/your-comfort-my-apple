using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using YGZ.Ordering.Api.Contracts;
using YGZ.BuildingBlocks.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using YGZ.Ordering.Application.Notifications.Commands.CreateNotification;

namespace YGZ.Ordering.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/orders/notification")]
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

    [HttpPost]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.Map<CreateNotificationCommand>(request);

        var result = await _sender.Send(command, cancellationToken);

        return result.Match(onSuccess: result => Ok(result), onFailure: HandleFailure);
    }
}
