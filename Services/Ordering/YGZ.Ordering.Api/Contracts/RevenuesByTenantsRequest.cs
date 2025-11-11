namespace YGZ.Ordering.Api.Contracts;

public sealed record RevenuesByTenantsRequest
{
    public List<string>? _tenants { get; init; }
    public DateTime? _startDate { get; init; }
    public DateTime? _endDate { get; init; }
}
