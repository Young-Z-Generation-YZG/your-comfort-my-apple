
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrderItemsByOrderId;

public sealed record GetOrderItemsByOrderIdQuery(string OrderId) : IQuery<OrderDetailsResponse> { }
