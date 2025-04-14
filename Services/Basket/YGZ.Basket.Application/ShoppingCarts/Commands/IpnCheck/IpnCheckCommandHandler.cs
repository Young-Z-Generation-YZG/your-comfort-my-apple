using YGZ.Basket.Application.Abstractions.Providers.vnpay;
using YGZ.BuildingBlocks.Shared.Abstractions.CQRS;
using YGZ.BuildingBlocks.Shared.Abstractions.Result;

namespace YGZ.Basket.Application.ShoppingCarts.Commands.IpnCheck;

public class IpnCheckCommandHandler : ICommandHandler<IpnCheckCommand, string>
{
    private readonly IVnpayProvider _vnpayProvider;

    public IpnCheckCommandHandler(IVnpayProvider vnpayProvider)
    {
        _vnpayProvider = vnpayProvider;
    }

    public async Task<Result<string>> Handle(IpnCheckCommand request, CancellationToken cancellationToken)
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

        if(result && request.ResponseCode == "00")
        {
            return "BasketCheckoutIntegrationEvent";
        } else
        {
            return "test";
        }
    }
}
