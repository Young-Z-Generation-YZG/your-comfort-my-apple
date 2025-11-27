using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;

namespace YGZ.Ordering.Application.Notifications.Commands.CreateNotification;

public sealed record CreateNotificationCommand : ICommand<bool>
{
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required string Type { get; init; }
    public required string Status { get; init; }
    public required string Link { get; init; }
}
