using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Notifications.Commands.MarkAsRead;

public sealed record MarkAsReadCommand : ICommand<bool>
{
    public required string NotificationId { get; init; }
}
