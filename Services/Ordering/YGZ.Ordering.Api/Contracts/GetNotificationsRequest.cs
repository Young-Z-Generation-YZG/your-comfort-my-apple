namespace YGZ.Ordering.Api.Contracts;

public sealed record GetNotificationsRequest
{
    public int? _page { get; init; } 
    public int? _limit { get; init; } 
    public List<string>? _types { get; init; }
    public List<string>? _statuses { get; init; }
    public bool? _isRead { get; init; }
}
