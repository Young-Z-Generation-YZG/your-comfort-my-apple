using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersByAdminHandler : IQueryHandler<GetOrdersByAdminQuery, PaginationResponse<OrderDetailsResponse>>
{
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserRequestContext _userContext;
    private readonly ILogger<GetOrdersByAdminHandler> _logger;

    public GetOrdersByAdminHandler(IGenericRepository<Order, OrderId> repository,
                                 IUserRequestContext userContext,
                                 ILogger<GetOrdersByAdminHandler> logger)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<PaginationResponse<OrderDetailsResponse>>> Handle(GetOrdersByAdminQuery request, CancellationToken cancellationToken)
    {
        var expression = BuildExpression(request);

        var result = await _repository.GetAllAsync(filterExpression: expression,
                                                   page: request.Page,
                                                   limit: request.Limit,
                                                   tracked: false,
                                                   includes: x => x.OrderItems,
                                                   cancellationToken: cancellationToken);

        var response = MapToResponse(result.items, result.totalRecords, result.totalPages, request);

        return response;
    }

    private static Expression<Func<Order, bool>> BuildExpression(GetOrdersByAdminQuery request)
    {
        var filterExpression = ExpressionBuilder.New<Order>();

        if (!string.IsNullOrWhiteSpace(request.CustomerName))
        {
            filterExpression = filterExpression.And(order => order.ShippingAddress.ContactName.Contains(request.CustomerName));
        }

        if (!string.IsNullOrWhiteSpace(request.OrderCode))
        {
            filterExpression = filterExpression.And(order => order.Code.Equals(Code.Of(request.OrderCode)));
        }

        EOrderStatus.TryFromName(request.OrderStatus, out var orderStatus);

        if (!string.IsNullOrWhiteSpace(request.OrderStatus) && orderStatus is not null)
        {
            filterExpression = filterExpression.And(order => order.OrderStatus == orderStatus);
        }

        return filterExpression;
    }

    private static PaginationResponse<OrderDetailsResponse> MapToResponse(List<Order> orders, int totalRecords, int totalPages, GetOrdersByAdminQuery request)
    {
        var queryParams = QueryParamBuilder.Build(request);

        var links = PaginationLinksBuilder.Build(basePath: "",
                                                 queryParams: queryParams,
                                                 currentPage: request.Page ?? 1,
                                                 totalPages: totalPages);

        var items = orders.Select(order => order.ToOrderDetailsResponse());

        var response = new PaginationResponse<OrderDetailsResponse>
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Items = items,
            Links = links
        };

        return response;
    }
}
