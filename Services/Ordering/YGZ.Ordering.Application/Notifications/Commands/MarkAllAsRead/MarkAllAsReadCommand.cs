using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Notifications.Commands.MarkAllAsRead;

public sealed record MarkAllAsReadCommand : ICommand<bool>
{
}
