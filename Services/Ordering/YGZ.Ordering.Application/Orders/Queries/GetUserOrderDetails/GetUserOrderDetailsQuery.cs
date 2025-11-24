using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;

namespace YGZ.Ordering.Application.Orders.Queries.GetUserOrderDetails;

public sealed record GetUserOrderDetailsQuery : IQuery<PaginationResponse<OrderDetailsResponse>>
{
    public required string UserId { get; init; }
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public string? OrderCode { get; init; }
    public string? OrderStatus { get; init; }
}

