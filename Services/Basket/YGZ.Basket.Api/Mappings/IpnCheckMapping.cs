using Mapster;
using YGZ.Basket.Api.Contracts;
using YGZ.Basket.Application.ShoppingCarts.Commands.IpnCheck;

namespace YGZ.Basket.Api.Mappings;

public class IpnCheckMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<IpnCheckRequest, IpnCheckCommand>()
            .Map(dest => dest.Amount, src => src.vnp_Amount)
            .Map(dest => dest.OrderInfo, src => src.vnp_OrderInfo)
            .Map(dest => dest.BankCode, src => src.vnp_BankCode)
            .Map(dest => dest.BankTranNo, src => src.vnp_BankTranNo)
            .Map(dest => dest.CardType, src => src.vnp_CardType)
            .Map(dest => dest.PayDate, src => src.vnp_PayDate)
            .Map(dest => dest.ResponseCode, src => src.vnp_ResponseCode)
            .Map(dest => dest.TmnCode, src => src.vnp_TmnCode)
            .Map(dest => dest.TransactionNo, src => src.vnp_TransactionNo)
            .Map(dest => dest.TransactionStatus, src => src.vnp_TransactionStatus)
            .Map(dest => dest.TxnRef, src => src.vnp_TxnRef)
            .Map(dest => dest.SecureHash, src => src.vnp_SecureHash);
    }
}
