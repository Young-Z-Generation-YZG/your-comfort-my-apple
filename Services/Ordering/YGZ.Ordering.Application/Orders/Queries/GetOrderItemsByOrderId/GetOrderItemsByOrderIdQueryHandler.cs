
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.Ordering.Application.Abstractions;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Orders.Queries.GetOrderItemsByOrderId;

public class GetOrderItemsByOrderIdQueryHandler : IQueryHandler<GetOrderItemsByOrderIdQuery, OrderDetailsResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserContext _userContext;

    public GetOrderItemsByOrderIdQueryHandler(IOrderRepository orderRepository, IUserContext userContext)
    {
        _orderRepository = orderRepository;
        _userContext = userContext;
    }

    public async Task<Result<OrderDetailsResponse>> Handle(GetOrderItemsByOrderIdQuery request, CancellationToken cancellationToken)
    {
        OrderId orderId = OrderId.Of(request.OrderId);

        var userId = UserId.Of(Guid.Parse(_userContext.GetUserId()));

        var orders = await _orderRepository.GetUserOrdersWithItemAsync(userId, cancellationToken);

        var order = orders.FirstOrDefault(x => x.Id == orderId);

        if (order is null)
        {
            return Errors.Order.DoesNotExist;
        }

        var response = MapToResponse(order);

        return response;
    }

    private OrderDetailsResponse MapToResponse(Order order)
    {
        var res = new OrderDetailsResponse()
        {
            OrderId = order.Id.Value.ToString(),
            OrderCode = order.Code.Value,
            OrderCustomerEmail = order.CustomerId.Value.ToString(),
            OrderStatus = order.Status.Name,
            OrderPaymentMethod = order.PaymentMethod.Name,
            OrderShippingAddress = new ShippingAddressResponse()
            {
                ContactName = order.ShippingAddress.ContactName,
                ContactEmail = order.ShippingAddress.ContactEmail,
                ContactPhoneNumber = order.ShippingAddress.ContactPhoneNumber,
                ContactAddressLine = order.ShippingAddress.AddressLine,
                ContactDistrict = order.ShippingAddress.District,
                ContactProvince = order.ShippingAddress.Province,
                ContactCountry = order.ShippingAddress.Country,
            },
            OrderItems = order.OrderItems.Select(x => new OrderItemRepsonse()
            {
                OrderItemId = x.Id.Value.ToString(),
                ProductId = x.ProductId,
                ModelId = x.ModelId,
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                ProductColorName = x.ProductColorName,
                ProductUnitPrice = x.ProductUnitPrice,
                Quantity = x.Quantity,
                SubTotalAmount = x.ProductUnitPrice * x.Quantity,
                IsReviewed = x.IsReviewed,
                Promotion = x.Promotion is not null ? new PromotionResponse()
                {
                    PromotionIdOrCode = x.Promotion.PromotionIdOrCode,
                    PromotionTitle = x.Promotion.PromotionTitle,
                    PromotionEventType = x.Promotion.PromotionEventType,
                    PromotionDiscountType = x.Promotion.PromotionDiscountType,
                    PromotionDiscountValue = x.Promotion.PromotionDiscountValue,
                    PromotionDiscountUnitPrice = x.Promotion.PromotionDiscountUnitPrice,
                    PromotionAppliedProductCount = x.Promotion.PromotionAppliedProductCount,
                    PromotionFinalPrice = x.Promotion.PromotionFinalPrice,
                } : null
            }).ToList(),
            OrderSubTotalAmount = order.SubTotalAmount,
            OrderDiscountAmount = order.DiscountAmount,
            OrderTotalAmount = order.TotalAmount,
            OrderCreatedAt = order.CreatedAt,
            OrderUpdatedAt = order.UpdatedAt,
            OrderLastModifiedBy = null
        };

        return res;
    }
}
