namespace YGZ.Ordering.Api.Contracts;

public sealed record RevenuesRequest
{
    public DateTime? _startDate { get; init; }
    public DateTime? _endDate { get; init; }
}
