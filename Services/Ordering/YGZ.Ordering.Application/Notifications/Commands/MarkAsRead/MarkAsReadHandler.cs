using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Ordering.Application.Notifications.Commands.MarkAsRead;

public class MarkAsReadHandler : ICommandHandler<MarkAsReadCommand, bool>
{
    private readonly ILogger<MarkAsReadHandler> _logger;

    public MarkAsReadHandler(ILogger<MarkAsReadHandler> logger)
    {
        _logger = logger;
    }

    public Task<Result<bool>> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
