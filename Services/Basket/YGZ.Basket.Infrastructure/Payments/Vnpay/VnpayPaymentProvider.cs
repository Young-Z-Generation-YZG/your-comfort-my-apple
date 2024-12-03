

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using YGZ.Basket.Application.Core.Abstractions.Payments;

namespace YGZ.Basket.Infrastructure.Payments.Vnpay;

public class VnpayPaymentProvider : IVnpayPaymentProvider
{
    public string VnpBaseUrl { get; private set; } = default!;
    public string VnpReturnUrl { get; private set; } = default!;
    public string VnpTmnCode { get; private set; } = default!;
    public string VnpHashSecret { get; private set; } = default!;
    public string VnpCommand { get; private set; } = default!;
    public string VnpCurrCode { get; private set; } = default!;
    public string VnpVersion { get; private set; } = default!;
    public string VnpLocale { get; private set; } = default!;
    public string VnpTimeZoneId { get; private set; } = default!;

    public VnpayPaymentProvider(IOptions<VnpaySettings> settings)
    {
        VnpBaseUrl = settings.Value.VnPayUrl;
        VnpReturnUrl = settings.Value.ReturnUrl;
        VnpTmnCode = settings.Value.TmnCode;
        VnpHashSecret = settings.Value.HashSecret;
        VnpCommand = settings.Value.Command;
        VnpCurrCode = settings.Value.CurrCode;
        VnpVersion = settings.Value.Version;
        VnpLocale = settings.Value.Locale;
        VnpTimeZoneId = settings.Value.TimeZoneId;
    }

    public string CreatePaymentUrl(PaymentInformationModel model, HttpContext context)
    {
        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(VnpTimeZoneId);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var tick = DateTime.Now.Ticks.ToString();

        var pay = new VnPayLibrary();

        var urlCallBack = VnpReturnUrl;

        pay.AddRequestData("vnp_Version", VnpVersion);
        pay.AddRequestData("vnp_Command", VnpCommand);
        pay.AddRequestData("vnp_TmnCode", VnpTmnCode);
        pay.AddRequestData("vnp_Amount", ((int)model.Amount * 100).ToString());
        pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        pay.AddRequestData("vnp_CurrCode", VnpCurrCode);
        pay.AddRequestData("vnp_IpAddr", pay.GetIpAddress(context));
        pay.AddRequestData("vnp_Locale", VnpLocale);
        pay.AddRequestData("vnp_OrderInfo", $"{model.Name} {model.OrderDescription} {model.Amount}");
        pay.AddRequestData("vnp_OrderType", model.OrderType);
        pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        pay.AddRequestData("vnp_TxnRef", tick);

        var paymentUrl =
            pay.CreateRequestUrl(VnpBaseUrl, VnpHashSecret);

        return paymentUrl;
    }

    public PaymentResponseModel PaymentExecute(IQueryCollection collections)
    {
        var pay = new VnPayLibrary();

        var response = pay.GetFullResponseData(collections, VnpHashSecret);

        return response;
    }
}
