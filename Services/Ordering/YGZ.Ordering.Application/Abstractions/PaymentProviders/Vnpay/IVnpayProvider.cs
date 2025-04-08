

namespace YGZ.Ordering.Application.Abstractions.PaymentProviders.Vnpay;

public interface IVnpayProvider
{
    public bool IpnCheck(Dictionary<string, string> vnpayResponseParams);
}
