using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;

namespace YGZ.Ordering.Application.Orders.Queries.RevenuesByTenants;

public sealed record RevenuesByTenantsQuery : IQuery<RevenuesByGroupResponse>
{
    public List<string>? Tenants { get; init; }
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}
