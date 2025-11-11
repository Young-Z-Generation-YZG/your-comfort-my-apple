using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;

namespace YGZ.Ordering.Application.Orders.Queries.RevenuesQuery;

public sealed record GetRevenuesQuery : IQuery<List<OrderDetailsResponse>>
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
}
