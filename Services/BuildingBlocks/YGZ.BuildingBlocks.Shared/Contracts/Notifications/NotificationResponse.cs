using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Notifications;

[JsonConverter(typeof(SnakeCaseJsonSerializer))]
public sealed record NotificationResponse
{
    public required string Id { get; init; }
    public string? SenderId { get; init; }
    public string? ReceiverId { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public required string Type { get; init; }
    public required string Status { get; init; }
    public bool IsRead { get; init; }
    public string? Link { get; init; }
    public bool IsSystem { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string? UpdatedBy { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime? DeletedAt { get; init; }
    public string? DeletedBy { get; init; }
}
