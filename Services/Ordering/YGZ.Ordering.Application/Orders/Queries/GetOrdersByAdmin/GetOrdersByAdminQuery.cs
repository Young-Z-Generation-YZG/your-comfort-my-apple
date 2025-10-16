using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrders;

public sealed record GetOrdersByAdminQuery : IQuery<PaginationResponse<OrderDetailsResponse>>
{
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public string? CustomerName { get; init; }
    public string? OrderCode { get; init; }
    public string? OrderStatus { get; init; }
}
