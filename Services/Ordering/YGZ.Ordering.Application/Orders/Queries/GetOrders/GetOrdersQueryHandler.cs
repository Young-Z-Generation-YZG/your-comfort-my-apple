using System.Linq.Expressions;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Builders;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Orders.ValueObjects;
using YGZ.Ordering.Infrastructure.Persistence.Expressions;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, PaginationResponse<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<PaginationResponse<OrderResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var expression = BuildExpression(request);

        var result = await _orderRepository.GetAllAsync(filterExpression: expression,
                                                        _page: request.Page,
                                                        _limit: request.Limit,
                                                        tracked: false,
                                                        cancellationToken: cancellationToken);


        var queryParams = new Dictionary<string, string>
        {
            { "_page", request.Page?.ToString() ?? "1" },
            { "_limit", request.Limit ?.ToString() ?? "10" },
            { "_orderName", request.OrderName! },
            { "_orderCode", request.OrderCode! },
            { "_orderStatus", request.OrderStatus! }
        };

        var links = PaginationLinksBuilder.Build(basePath: "/api/v1/orders",
                                                 queryParams: queryParams,
                                                 currentPage: request.Page ?? 1,
                                                 totalPages: result.totalPages);

        var response = new PaginationResponse<OrderResponse>
        {
            TotalRecords = result.totalRecords,
            TotalPages = result.totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Data = new List<OrderResponse>(),
            Links = links
        };

        return response;
    }

    private static Expression<Func<Order, bool>> BuildExpression(GetOrdersQuery request)
    {
        var filterExpression = ExpressionBuilder.New<Order>();

        if (!string.IsNullOrWhiteSpace(request.OrderName))
        {
            filterExpression = filterExpression.And(order => order.ShippingAddress.ContactName.Contains(request.OrderName));
        }

        if (!string.IsNullOrWhiteSpace(request.OrderCode))
        {
            filterExpression = filterExpression.And(order => order.Code.Equals(Code.Of(request.OrderCode)));
        }

        if (!string.IsNullOrWhiteSpace(request.OrderStatus))
        {
            filterExpression = filterExpression.And(order => order.Status == OrderStatusEnum.FromName(request.OrderStatus, false));
        }

        return filterExpression;
    }
}
