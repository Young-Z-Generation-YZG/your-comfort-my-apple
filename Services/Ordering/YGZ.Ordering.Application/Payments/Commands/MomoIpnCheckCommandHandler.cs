
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Abstractions.PaymentProviders.Momo;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Payments.Commands;

public class MomoIpnCheckCommandHandler : ICommandHandler<MomoIpnCheckCommand, OrderDetailsResponse>
{
    private readonly IMomoProvider _momoProvider;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<MomoIpnCheckCommandHandler> _logger;

    public MomoIpnCheckCommandHandler(IMomoProvider momoProvider, IOrderRepository orderRepository, ILogger<MomoIpnCheckCommandHandler> logger)
    {
        _momoProvider = momoProvider;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Result<OrderDetailsResponse>> Handle(MomoIpnCheckCommand request, CancellationToken cancellationToken)
    {
        Dictionary<string, string> responseParams = new Dictionary<string, string>()
        {
            { "partnerCode", request.PartnerCode },
            { "accessKey", request.AccessKey },
            { "requestId", request.RequestId },
            { "amount", request.Amount },
            { "orderId", request.OrderId },
            { "orderInfo", request.OrderInfo },
            { "orderType", request.OrderType },
            { "transId", request.TransId },
            { "message", request.Message },
            { "localMessage", request.LocalMessage },
            { "responseTime", request.ResponseTime },
            { "errorCode", request.ErrorCode },
            { "payType", request.PayType },
            { "extraData", request.ExtraData },
            { "signature", request.Signature },
        };
         
        var result = _momoProvider.IpnCheck(responseParams);

        if (result && request.ErrorCode == "0")
        {
            var orderId = request.OrderId.Split("ORDER_ID=")[1];

            if (orderId is null)
            {
                _logger.LogError("orderId can not get orderId");

                return Errors.Order.DoesNotExist;
            }

            var order = await _orderRepository.GetOrderByIdWithInclude(OrderId.Of(orderId), (o => o.OrderItems), cancellationToken);

            if (order is not null && OrderStatus.Equals(order.Status, OrderStatus.PENDING))
            {
                order.Status = OrderStatus.PAID;

                var updatedResult = await _orderRepository.UpdateAsync(order, cancellationToken);

                OrderDetailsResponse response = MapToResponse(order);

                return response;
            }
            else if (order is not null && OrderStatus.Equals(order.Status, OrderStatus.PAID))
            {
                OrderDetailsResponse response = MapToResponse(order);

                return response;
            }
        }

        return Errors.Payment.PaymentFailure;
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
