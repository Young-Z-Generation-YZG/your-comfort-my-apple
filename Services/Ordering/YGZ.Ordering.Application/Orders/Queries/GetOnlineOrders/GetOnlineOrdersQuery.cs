using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;

namespace YGZ.Ordering.Application.Orders.Queries.GetOnlineOrders;

public sealed record GetOnlineOrdersQuery : IQuery<PaginationResponse<OrderDetailsResponse>>
{
    public int? _page { get; init; } = 1;
    public int? _limit { get; init; } = 10;
    public List<string>? _paymentMethod { get; init; } = new List<string>();
    public List<string>? _orderStatus { get; init; } = new List<string>();
    public string? _orderCode { get; init; }
    public string? _customerEmail { get; init; }
    public string? _customerName { get; init; }
    public string? _customerPhoneNumber { get; init; }
}
