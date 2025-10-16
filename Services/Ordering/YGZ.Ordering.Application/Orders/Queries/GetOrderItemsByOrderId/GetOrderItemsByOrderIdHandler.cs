
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrderItemsByOrderId;

public class GetOrderItemsByOrderIdHandler : IQueryHandler<GetOrderItemsByOrderIdQuery, OrderDetailsResponse>
{
    private readonly IGenericRepository<Order, OrderId> _repository;
    private readonly IUserRequestContext _userContext;
    private readonly ILogger<GetOrderItemsByOrderIdHandler> _logger;

    public GetOrderItemsByOrderIdHandler(IUserRequestContext userContext,
                                         IGenericRepository<Order, OrderId> repository,
                                         ILogger<GetOrderItemsByOrderIdHandler> logger)
    {
        _repository = repository;
        _userContext = userContext;
        _logger = logger;
    }

    public async Task<Result<OrderDetailsResponse>> Handle(GetOrderItemsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();

        //OrderId orderId = OrderId.Of(request.OrderId);

        //var roles = _userContext.GetUserRoles();

        //if (roles.Contains(ROLES.ADMIN))
        //{
        //    var adminOrder = await _orderRepository.GetOrderByIdWithInclude(orderId, x => x.OrderItems, cancellationToken);

        //    if (adminOrder is null)
        //    {
        //        return Errors.Order.DoesNotExist;
        //    }

        //    return MapToResponse(adminOrder, adminOrder.ShippingAddress.ContactEmail);
        //}

        //var userId = UserId.Of(Guid.Parse(_userContext.GetUserId()));

        //var userEmail = _userContext.GetUserEmail();

        //var orders = await _orderRepository.GetUserOrdersWithItemAsync(userId, cancellationToken);

        //var userOrder = orders.FirstOrDefault(x => x.Id == orderId);

        //if (userOrder is null)
        //{
        //    return Errors.Order.DoesNotExist;
        //}


        //return MapToResponse(userOrder, userEmail);
    }

    //private OrderDetailsResponse MapToResponse(Order order, string userEmail)
    //{
    //    var res = new OrderDetailsResponse()
    //    {
    //        OrderId = order.Id.Value.ToString(),
    //        OrderCode = order.Code.Value,
    //        OrderCustomerEmail = userEmail,
    //        OrderStatus = order.Status.Name,
    //        OrderPaymentMethod = order.PaymentMethod.Name,
    //        OrderShippingAddress = new ShippingAddressResponse()
    //        {
    //            ContactName = order.ShippingAddress.ContactName,
    //            ContactEmail = order.ShippingAddress.ContactEmail,
    //            ContactPhoneNumber = order.ShippingAddress.ContactPhoneNumber,
    //            ContactAddressLine = order.ShippingAddress.AddressLine,
    //            ContactDistrict = order.ShippingAddress.District,
    //            ContactProvince = order.ShippingAddress.Province,
    //            ContactCountry = order.ShippingAddress.Country,
    //        },
    //        OrderItems = order.OrderItems.Select(x => new OrderItemRepsonse()
    //        {
    //            OrderId = order.Id.Value.ToString(),
    //            OrderItemId = x.Id.Value.ToString(),
    //            ProductId = x.ProductId,
    //            ModelId = x.ModelId,
    //            ProductName = x.ProductName,
    //            ProductImage = x.ProductImage,
    //            ProductColorName = x.ProductColorName,
    //            ProductUnitPrice = x.ProductUnitPrice,
    //            Quantity = x.Quantity,
    //            SubTotalAmount = x.ProductUnitPrice * x.Quantity,
    //            IsReviewed = x.IsReviewed,
    //            Promotion = x.Promotion is not null ? new PromotionResponse()
    //            {
    //                PromotionIdOrCode = x.Promotion.PromotionIdOrCode,
    //                PromotionTitle = x.Promotion.PromotionTitle,
    //                PromotionEventType = x.Promotion.PromotionEventType,
    //                PromotionDiscountType = x.Promotion.PromotionDiscountType,
    //                PromotionDiscountValue = x.Promotion.PromotionDiscountValue,
    //                PromotionDiscountUnitPrice = x.Promotion.PromotionDiscountUnitPrice,
    //                PromotionAppliedProductCount = x.Promotion.PromotionAppliedProductCount,
    //                PromotionFinalPrice = x.Promotion.PromotionFinalPrice,
    //            } : null
    //        }).ToList(),
    //        OrderSubTotalAmount = order.SubTotalAmount,
    //        OrderDiscountAmount = order.DiscountAmount,
    //        OrderTotalAmount = order.TotalAmount,
    //        OrderCreatedAt = order.CreatedAt,
    //        OrderUpdatedAt = order.UpdatedAt,
    //        OrderLastModifiedBy = null
    //    };

    //    return res;
    //}
}
