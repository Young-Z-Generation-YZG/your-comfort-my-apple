using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Notifications.Commands.MarkAsRead;

public sealed record MarkAsReadCommand : ICommand<bool>
{
}
