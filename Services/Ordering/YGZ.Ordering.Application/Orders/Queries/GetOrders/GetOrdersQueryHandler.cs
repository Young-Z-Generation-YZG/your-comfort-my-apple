﻿ using System.Linq.Expressions;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Common;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler : IQueryHandler<GetOrdersQuery, PaginationResponse<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserContext _userContext;

    public GetOrdersQueryHandler(IOrderRepository orderRepository, IUserContext userContext)
    {
        _orderRepository = orderRepository;
        _userContext = userContext;
    }

    public async Task<Result<PaginationResponse<OrderResponse>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var expression = BuildExpression(request);

        var result = await _orderRepository.GetAllAsync(filterExpression: expression,
                                                        _page: request.Page,
                                                        _limit: request.Limit,
                                                        tracked: false,
                                                        cancellationToken: cancellationToken,
                                                        includes: x => x.OrderItems);

        var response = MapToResponse(result.orders, result.totalRecords, result.totalPages, request);

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
            filterExpression = filterExpression.And(order => order.Status == OrderStatus.FromName(request.OrderStatus, false));
        }

        return filterExpression;
    }

    private PaginationResponse<OrderResponse> MapToResponse(List<Order> orders, int totalRecords, int totalPages, GetOrdersQuery request)
    {
        var queryParams = QueryParamBuilder.Build(request);

        var links = PaginationLinksBuilder.Build(basePath: "/api/v1/orders",
                                                 queryParams: queryParams,
                                                 currentPage: request.Page ?? 1,
                                                 totalPages: totalPages);

        var items = orders.Select(order => new OrderResponse
        {
            OrderId = order.Id.Value.ToString(),
            OrderCode = order.Code.Value,
            OrderCustomerEmail = order.CustomerId.Value.ToString(),
            OrderStatus = order.Status.Name,
            OrderPaymentMethod = order.PaymentMethod.Name,
            OrderShippingAddress = new ShippingAddressResponse
            {
                ContactName = order.ShippingAddress.ContactName,
                ContactEmail = order.ShippingAddress.ContactEmail,
                ContactPhoneNumber = order.ShippingAddress.ContactPhoneNumber,
                ContactAddressLine = order.ShippingAddress.Country,
                ContactDistrict = order.ShippingAddress.District,
                ContactProvince = order.ShippingAddress.Province,
                ContactCountry = order.ShippingAddress.Country
            },
            OrderItemsCount = order.OrderItems.Count,
            OrderSubTotalAmount = order.SubTotalAmount,
            OrderDiscountAmount = order.DiscountAmount,
            OrderTotalAmount = order.TotalAmount,
            OrderCreatedAt = order.CreatedAt,
            OrderUpdatedAt = order.UpdatedAt,
            OrderLastModifiedBy = null
        }).ToList();

        var response = new PaginationResponse<OrderResponse>
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
