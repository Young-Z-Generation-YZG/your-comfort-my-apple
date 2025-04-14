
using Microsoft.AspNetCore.Http;

namespace YGZ.Basket.Application.Abstractions.Providers.vnpay;

public interface IVnpayProvider
{
    public string CreatePaymentUrl(VnpayInformationModel model, HttpContext context);
    public PaymentResponseModel PaymentExecute(IQueryCollection collections);
    public bool IpnCheck(Dictionary<string, string> vnpayResponseParams);
}
