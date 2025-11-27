using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Ordering.Application.Notifications.Commands.MarkAllAsRead;

public class MarkAllAsReadHandler : ICommandHandler<MarkAllAsReadCommand, bool>
{
    private readonly ILogger<MarkAllAsReadHandler> _logger;

    public MarkAllAsReadHandler(ILogger<MarkAllAsReadHandler> logger)
    {
        _logger = logger;
    }

    public Task<Result<bool>> Handle(MarkAllAsReadCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
