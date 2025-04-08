
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;
using YGZ.Ordering.Application.Abstractions.Data;
using YGZ.Ordering.Application.Abstractions.PaymentProviders.Vnpay;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Application.Payments.Commands;

public class IpnCheckCommandHandler : ICommandHandler<IpnCheckCommand, bool>
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

    public async Task<Result<bool>> Handle(IpnCheckCommand request, CancellationToken cancellationToken)
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

                return false;
            }

            var order = await _orderRepository.GetOrderByIdWithInclude(OrderId.Of(orderId), (o => o.OrderItems),  cancellationToken);

            if (order is not null && OrderStatusEnum.Equals(order.Status, OrderStatusEnum.PENDING))
            {
                order.Status = OrderStatusEnum.PAID;

                var updatedResult = await _orderRepository.UpdateAsync(order, cancellationToken);

                return updatedResult;
            }
        }

        return false;
    }
}
