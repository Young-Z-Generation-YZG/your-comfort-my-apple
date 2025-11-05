namespace YGZ.Catalog.Api.Contracts.TenantRequest;

public sealed record GetTenantsRequest
{
    public int? _page { get; init; } = 1;
    public int? _limit { get; init; } = 10;
    public string? _tenantName { get; init; }
    public string? _tenantType { get; init; }
}
