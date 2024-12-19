

using Microsoft.AspNetCore.Http;
using YGZ.Basket.Infrastructure.Payments.Vnpay;

namespace YGZ.Basket.Application.Core.Abstractions.Payments;

public interface IVnpayPaymentProvider
{
    string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}
