using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.GetUserOrderDetails;

public sealed class GetUserOrderDetailsHandler : IQueryHandler<GetUserOrderDetailsQuery, PaginationResponse<OrderDetailsResponse>>
{
    private readonly ILogger<GetUserOrderDetailsHandler> _logger;
    private readonly IGenericRepository<Order, OrderId> _repository;

    public GetUserOrderDetailsHandler(ILogger<GetUserOrderDetailsHandler> logger,
                                      IGenericRepository<Order, OrderId> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<Result<PaginationResponse<OrderDetailsResponse>>> Handle(GetUserOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var filterExpression = BuildExpression(request);

            Expression<Func<Order, object>>[] includeExpressions =
            [
                order => order.OrderItems
            ];

            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderByCreatedAtDesc =
                query => query.OrderByDescending(order => order.CreatedAt);

            var result = await _repository.GetAllAsync(filterExpression: filterExpression,
                                                       includeExpressions: includeExpressions,
                                                       orderBy: orderByCreatedAtDesc,
                                                       page: request.Page,
                                                       limit: request.Limit,
                                                       cancellationToken: cancellationToken);

            var response = MapToResponse(result.items, result.totalRecords, result.totalPages, request);

            _logger.LogInformation("::[Operation Information]:: Method: {MethodName}, Information message: {InformationMessage}, Parameters: {@Parameters}",
                nameof(Handle), "Successfully retrieved user order details", new { userId = request.UserId, totalRecords = result.totalRecords, page = request.Page, limit = request.Limit });

            return response;
        }
        catch (Exception exception)
        {
            var parameters = new { userId = request.UserId, page = request.Page, limit = request.Limit };
            _logger.LogError(exception, ":[Application Exception]: Method: {MethodName}, Error message: {ErrorMessage}, Parameters: {@Parameters}",
                nameof(Handle), exception.Message, parameters);
            throw;
        }
    }

    private static Expression<Func<Order, bool>> BuildExpression(GetUserOrderDetailsQuery request)
    {
        var filterExpression = ExpressionBuilder.New<Order>();

        var userId = UserId.Of(request.UserId);

        filterExpression = filterExpression.And(order => order.CustomerId == userId);

        //if (!string.IsNullOrWhiteSpace(request.OrderCode))
        //{
        //    var code = Code.Of(request.OrderCode);
        //    filterExpression = filterExpression.And(order => order.Code == code);
        //}

        //if (!string.IsNullOrWhiteSpace(request.OrderStatus) &&
        //    EOrderStatus.TryFromName(request.OrderStatus, true, out var orderStatus))
        //{
        //    filterExpression = filterExpression.And(order => order.OrderStatus == orderStatus);
        //}

        return filterExpression;
    }

    private static PaginationResponse<OrderDetailsResponse> MapToResponse(
        List<Order> orders,
        int totalRecords,
        int totalPages,
        GetUserOrderDetailsQuery request)
    {
        var queryParams = QueryParamBuilder.Build(request);
        queryParams.Remove("_userId");

        var links = PaginationLinksBuilder.Build(
            basePath: string.Empty,
            queryParams: queryParams,
            currentPage: request.Page ?? 1,
            totalPages: totalPages);

        var response = new PaginationResponse<OrderDetailsResponse>
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            PageSize = request.Limit ?? 10,
            CurrentPage = request.Page ?? 1,
            Items = orders.Select(order => order.ToResponse()),
            Links = links
        };

        return response;
    }
}

