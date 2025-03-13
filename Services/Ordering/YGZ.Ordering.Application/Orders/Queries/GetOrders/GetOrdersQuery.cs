using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery : IQuery<PaginationResponse<OrderResponse>>
{
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public string? OrderName { get; set; }
    public string? OrderCode { get; set; }
    public string? OrderStatus { get; set; }
}
