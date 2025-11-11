namespace YGZ.Ordering.Api.Contracts;

public sealed record RevenuesByYearsRequest
{
    public List<string>? _years { get; init; }
}
