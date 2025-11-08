using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrderByUser;

public class GetOrdersByUserHandler : IQueryHandler<GetOrdersByUserQuery, PaginationResponse<OrderDetailsResponse>>
{
    private readonly ILogger<GetOrdersByUserHandler> _logger;
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserHttpContext _userContext;

    public GetOrdersByUserHandler(ILogger<GetOrdersByUserHandler> logger,
                                  IGenericRepository<Order, OrderId> repository,
                                  IUserHttpContext userContext)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<PaginationResponse<OrderDetailsResponse>>> Handle(GetOrdersByUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.GetUserId();

        var filterExpression = BuildExpression(request, UserId.Of(userId));

        var includeExpressions = new Expression<Func<Order, object>>[]
        {
            x => x.OrderItems
        };

        var result = await _repository.GetAllAsync(filterExpression: filterExpression,
                                                   includeExpressions: includeExpressions,
                                                   page: request.Page,
                                                   limit: request.Limit,
                                                   cancellationToken: cancellationToken);

        var response = MapToResponse(result.items, result.totalRecords, result.totalPages, request);

        return response;
    }

    private static Expression<Func<Order, bool>> BuildExpression(GetOrdersByUserQuery request, UserId userId)
    {
        var filterExpression = ExpressionBuilder.New<Order>();

        filterExpression = filterExpression.And(order => order.CustomerId == userId);


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

    private PaginationResponse<OrderDetailsResponse> MapToResponse(List<Order> orders, int totalRecords, int totalPages, GetOrdersByUserQuery request)
    {
        var queryParams = QueryParamBuilder.Build(request);

        var links = PaginationLinksBuilder.Build(basePath: "",
                                                 queryParams: queryParams,
                                                 currentPage: request.Page ?? 1,
                                                 totalPages: totalPages);

        var items = orders.Select(order => order.ToResponse());

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
