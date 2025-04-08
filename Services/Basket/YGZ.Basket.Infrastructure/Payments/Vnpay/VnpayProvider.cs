
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using YGZ.Basket.Application.Abstractions.Providers.vnpay;
using YGZ.Basket.Infrastructure.Payments.Vnpay.Lib;
using YGZ.Basket.Infrastructure.Settings;

namespace YGZ.Basket.Infrastructure.Payments.Vnpay;

public class VnpayProvider : IVnpayProvider
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

    public string WebClientBaseUrl { get; private set; } = default!;

    private VnPayLibrary Vnpay = new VnPayLibrary();

    public VnpayProvider(IOptions<VnpaySettings> vnpaySettings, IOptions<WebClientSettings> webClientSettings)
    {
        VnpBaseUrl = vnpaySettings.Value.VnPayUrl;
        VnpReturnUrl = vnpaySettings.Value.ReturnUrl;
        VnpTmnCode = vnpaySettings.Value.TmnCode;
        VnpHashSecret = vnpaySettings.Value.HashSecret;
        VnpCommand = vnpaySettings.Value.Command;
        VnpCurrCode = vnpaySettings.Value.CurrCode;
        VnpVersion = vnpaySettings.Value.Version;
        VnpLocale = vnpaySettings.Value.Locale;
        VnpTimeZoneId = vnpaySettings.Value.TimeZoneId;

        WebClientBaseUrl = webClientSettings.Value.BaseUrl;
    }

    public string CreatePaymentUrl(VnpayInformationModel model, HttpContext context)
    {
        Vnpay.ClearData();

        var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(VnpTimeZoneId);
        var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
        var tick = DateTime.Now.Ticks.ToString();

        var urlCallBack = $"{WebClientBaseUrl}/{VnpReturnUrl}";

        var amount = ((double)model.Amount * 100).ToString();

        Vnpay.AddRequestData("vnp_Version", VnpVersion);
        Vnpay.AddRequestData("vnp_Command", VnpCommand);
        Vnpay.AddRequestData("vnp_TmnCode", VnpTmnCode);
        Vnpay.AddRequestData("vnp_Amount", amount.ToString());
        Vnpay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
        Vnpay.AddRequestData("vnp_CurrCode", VnpCurrCode);
        Vnpay.AddRequestData("vnp_IpAddr", Vnpay.GetIpAddress(context));
        Vnpay.AddRequestData("vnp_Locale", VnpLocale);
        Vnpay.AddRequestData("vnp_OrderInfo", $"{model.OrderDescription}");
        Vnpay.AddRequestData("vnp_OrderType", model.OrderType);
        Vnpay.AddRequestData("vnp_ReturnUrl", urlCallBack);
        Vnpay.AddRequestData("vnp_TxnRef", tick);

        var paymentUrl =
            Vnpay.CreateRequestUrl(VnpBaseUrl, VnpHashSecret);

        return paymentUrl;
    }

    public PaymentResponseModel PaymentExecute(IQueryCollection collections)
    {
        var pay = new VnPayLibrary();

        var response = pay.GetFullResponseData(collections, VnpHashSecret);

        return response;
    }

    public bool IpnCheck(Dictionary<string, string> vnpayResponseParams)
    {
        Vnpay.ClearData();

        for (int i = 0; i < vnpayResponseParams.Count; i++)
        {
            var key = vnpayResponseParams.ElementAt(i).Key;
            var value = vnpayResponseParams.ElementAt(i).Value;
            Vnpay.AddResponseData(key, value);
        }

        var vnpSecureHash = vnpayResponseParams.FirstOrDefault(k => k.Key == "vnp_SecureHash").Value;

        var checkSignature = Vnpay.ValidateSignature(vnpSecureHash, VnpHashSecret);

        return checkSignature;
    }
}
