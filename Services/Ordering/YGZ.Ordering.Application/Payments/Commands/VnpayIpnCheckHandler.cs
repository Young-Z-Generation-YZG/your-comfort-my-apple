
using System.Linq.Expressions;
using System.Web;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.BuildingBlocks.Shared.Contracts.Ordering;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Abstractions.PaymentProviders.Vnpay;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Errors;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Payments.Commands;

public class VnpayIpnCheckHandler : ICommandHandler<VnpayIpnCheckCommand, OrderDetailsResponse>
{
    private readonly ILogger<VnpayIpnCheckHandler> _logger;
    private readonly IVnpayProvider _vnpayProvider;
    private readonly IGenericRepository<Order, OrderId> _repository;

    public VnpayIpnCheckHandler(IVnpayProvider vnpayProvider,
                                IGenericRepository<Order, OrderId> repository,
                                ILogger<VnpayIpnCheckHandler> logger)
    {
        _vnpayProvider = vnpayProvider;
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<OrderDetailsResponse>> Handle(VnpayIpnCheckCommand request, CancellationToken cancellationToken)
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

        if (request.ResponseCode == "00")
        {
            var decodedOrderInfo = HttpUtility.UrlDecode(request.OrderInfo);
            var orderId = decodedOrderInfo.Split("ORDER_ID=")[1];

            if (orderId is null)
            {
                _logger.LogError("orderId can not get orderId");

                return Errors.Order.DoesNotExist;
            }

            var expressions = new Expression<Func<Order, object>>[]
            {
                x => x.OrderItems
            };

            var order = await _repository.GetByIdAsync(OrderId.Of(orderId), expressions, cancellationToken);

            if (order is null)
            {
                return Errors.Order.DoesNotExist;
            }

            if (order is not null && EOrderStatus.Equals(order.OrderStatus, EOrderStatus.PENDING))
            {
                order.SetPaid();

                var updateResult = await _repository.UpdateAsync(order, cancellationToken);

                if (updateResult.IsFailure)
                {
                    return updateResult.Error;
                }

                OrderDetailsResponse response = order.ToResponse();

                return response;
            }
            else if (order is not null && EOrderStatus.Equals(order.OrderStatus, EOrderStatus.PAID))
            {
                OrderDetailsResponse response = order.ToResponse();

                return response;
            }
        }

        return Errors.Payment.PaymentFailure;
    }
}