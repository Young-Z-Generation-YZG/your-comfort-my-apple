
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Baskets;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Abstractions.PaymentProviders.Vnpay;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Payments.Commands;

public class IpnCheckCommandHandler : ICommandHandler<IpnCheckCommand, OrderDetailsResponse>
{
    private readonly IVnpayProvider _vnpayProvider;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<IpnCheckCommandHandler> _logger;

    public IpnCheckCommandHandler(IVnpayProvider vnpayProvider, IOrderRepository orderRepository, ILogger<IpnCheckCommandHandler> logger)
    {
        _vnpayProvider = vnpayProvider;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Result<OrderDetailsResponse>> Handle(IpnCheckCommand request, CancellationToken cancellationToken)
    {
        Dictionary<string, string> responseParams = new Dictionary<string, string>()
        {
            { "vnp_Amount", request.Amount },
            { "vnp_OrderInfo", request.OrderInfo },
            { "vnp_BankCode", request.BankCode },
            { "vnp_BankTranNo", request.BankTranNo },
            { "vnp_CardType", request.CardType },
            { "vnp_PayDate", request.PayDate },
            { "vnp_ResponseCode", request.ResponseCode },
            { "vnp_TmnCode", request.TmnCode },
            { "vnp_TransactionNo", request.TransactionNo },
            { "vnp_TransactionStatus", request.TransactionStatus },
            { "vnp_TxnRef", request.TxnRef },
            { "vnp_SecureHash", request.SecureHash }
        };

        var result = _vnpayProvider.IpnCheck(responseParams);

        if (result && request.ResponseCode == "00")
        {
            var orderId = request.OrderInfo.Split("ORDER_ID=")[1];

            if(orderId is null)
            {
                _logger.LogError("orderId can not get orderId");

                return Errors.Order.DoesNotExist;
            }

            var order = await _orderRepository.GetOrderByIdWithInclude(OrderId.Of(orderId), (o => o.OrderItems),  cancellationToken);

            if (order is not null && OrderStatusEnum.Equals(order.Status, OrderStatusEnum.PENDING))
            {
                order.Status = OrderStatusEnum.PAID;

                var updatedResult = await _orderRepository.UpdateAsync(order, cancellationToken);

                OrderDetailsResponse response = MapToResponse(order);

                return response;
            } 
            else if(order is not null && OrderStatusEnum.Equals(order.Status, OrderStatusEnum.PAID))
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
                ProductName = x.ProductName,
                ProductImage = x.ProductImage,
                ProductColorName = x.ProductColorName,
                ProductUnitPrice = x.ProductUnitPrice,
                quantity = x.Quantity,
                SubTotalAmount = x.ProductUnitPrice * x.Quantity,
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