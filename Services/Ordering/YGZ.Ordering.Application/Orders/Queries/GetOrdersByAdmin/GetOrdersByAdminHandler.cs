using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersByAdminHandler : IQueryHandler<GetOrdersByAdminQuery, PaginationResponse<OrderDetailsResponse>>
{
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserHttpContext _userContext;
    private readonly ILogger<GetOrdersByAdminHandler> _logger;

    public GetOrdersByAdminHandler(IGenericRepository<Order, OrderId> repository,
                                   IUserHttpContext userContext,
                                   ILogger<GetOrdersByAdminHandler> logger)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<PaginationResponse<OrderDetailsResponse>>> Handle(GetOrdersByAdminQuery request, CancellationToken cancellationToken)
    {
        var filterExpression = BuildExpression(request);

        var includeExpressions = new Expression<Func<Order, object>>[]
        {
            x => x.OrderItems
        };

        Func<IQueryable<Order>, IOrderedQueryable<Order>> orderByCreatedAtAsc = query => query.OrderByDescending(order => order.CreatedAt);

        var result = await _repository.GetAllAsync(filterExpression: filterExpression,
                                                   orderBy: orderByCreatedAtAsc,
                                                   page: request._page,
                                                   limit: request._limit,
                                                   includeExpressions: includeExpressions,
                                                   cancellationToken: cancellationToken);

        var response = MapToResponse(result.items, result.totalRecords, result.totalPages, request);

        return response;
    }

    private static Expression<Func<Order, bool>> BuildExpression(GetOrdersByAdminQuery request)
    {
        var filterExpression = ExpressionBuilder.New<Order>();

        if (!string.IsNullOrWhiteSpace(request._customerEmail))
        {
            filterExpression = filterExpression.And(order => order.ShippingAddress.ContactEmail.Contains(request._customerEmail));
        }

        if (!string.IsNullOrWhiteSpace(request._customerName))
        {
            filterExpression = filterExpression.And(order => order.ShippingAddress.ContactName.Contains(request._customerName));
        }

        if (!string.IsNullOrWhiteSpace(request._customerPhoneNumber))
        {
            filterExpression = filterExpression.And(order => order.ShippingAddress.ContactPhoneNumber.Contains(request._customerPhoneNumber));
        }

        if (!string.IsNullOrWhiteSpace(request._orderCode))
        {
            filterExpression = filterExpression.And(order => order.Code.Equals(Code.Of(request._orderCode)));
        }

        // Filter by order status list
        if (request._orderStatus is not null && request._orderStatus.Any())
        {
            var orderStatuses = new List<EOrderStatus>();
            foreach (var status in request._orderStatus)
            {
                if (EOrderStatus.TryFromName(status, out var orderStatus))
                {
                    orderStatuses.Add(orderStatus);
                }
            }

            if (orderStatuses.Any())
            {
                filterExpression = filterExpression.And(order => orderStatuses.Contains(order.OrderStatus));
            }
        }

        // Filter by payment method list
        if (request._paymentMethod is not null && request._paymentMethod.Any())
        {
            var paymentMethods = new List<EPaymentMethod>();
            foreach (var method in request._paymentMethod)
            {
                if (EPaymentMethod.TryFromName(method, out var paymentMethod))
                {
                    paymentMethods.Add(paymentMethod);
                }
            }

            if (paymentMethods.Any())
            {
                filterExpression = filterExpression.And(order => paymentMethods.Contains(order.PaymentMethod));
            }
        }

        return filterExpression;
    }

    private static PaginationResponse<OrderDetailsResponse> MapToResponse(List<Order> orders, int totalRecords, int totalPages, GetOrdersByAdminQuery request)
    {
        var links = PaginationLinksBuilder.Build(
            basePath: "",
            request: request,
            currentPage: request._page,
            totalPages: totalPages);

        var items = orders.Select(order => order.ToResponse());

        var response = new PaginationResponse<OrderDetailsResponse>
        {
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            PageSize = request._limit ?? 10,
            CurrentPage = request._page ?? 1,
            Items = items,
            Links = links
        };

        return response;
    }
}
