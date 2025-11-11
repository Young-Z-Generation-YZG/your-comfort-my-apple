using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;

namespace YGZ.Ordering.Application.Orders.Queries.RevenuesByYears;

public class RevenuesByYearsQuery : IQuery<RevenuesByGroupResponse>
{
    public List<string>? Years { get; init; }
}
