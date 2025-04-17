
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using YGZ.Ordering.Application.Abstractions.PaymentProviders.Momo;
using YGZ.Ordering.Infrastructure.Settings;

namespace YGZ.Ordering.Infrastructure.Payments.Momo;

public class MomoProvider : IMomoProvider
{
    private readonly MomoSettings _momoSettings;
    private readonly WebClientSettings _webClientSettings;

    public MomoProvider(IOptions<MomoSettings> momoSettings, IOptions<WebClientSettings> webSettings)
    {
        _momoSettings = momoSettings.Value;
        _webClientSettings = webSettings.Value;
    }

    public bool IpnCheck(Dictionary<string, string> MomoResponseParams)
    {
        var callBackUrl = $"{_webClientSettings.BaseUrl}/{_momoSettings.ReturnUrl}";

        var requestId = MomoResponseParams.First(s => s.Key == "requestId").Value;
        var amount = MomoResponseParams.First(s => s.Key == "amount").Value;
        var orderInfo = MomoResponseParams.First(s => s.Key == "orderInfo").Value;
        var orderId = MomoResponseParams.First(s => s.Key == "orderId").Value;
        var errorCode = MomoResponseParams.First(s => s.Key == "errorCode").Value;
        var signature = MomoResponseParams.First(s => s.Key == "signature").Value;

        var rawData = $"partnerCode={_momoSettings.PartnerCode}"
            + $"&accessKey={_momoSettings.AccessKey}"
            + $"&requestId={requestId}"
            + $"&amount={amount}"
            + $"&orderId={orderId}"
            + $"&orderInfo={orderInfo}"
            + $"&returnUrl={callBackUrl}"
            + $"&notifyUrl={_momoSettings.NotifyUrl}"
            + $"&extraData=";

        if (errorCode != "0")
        {
            return false;
        }

        var secretKey = _momoSettings.SecrectKey;
        if (secretKey != null)
        {
            var hash = ComputeHmacSha256(rawData, secretKey);
            var result = hash.Equals(signature);
        }

        return true;
    }
    private string ComputeHmacSha256(string rawData, string secretKey)
    {
        var keyBytes = Encoding.UTF8.GetBytes(secretKey);
        var messageBytes = Encoding.UTF8.GetBytes(rawData);

        byte[] hashBytes;

        using (var hmac = new HMACSHA256(keyBytes))
        {
            hashBytes = hmac.ComputeHash(messageBytes);
        }

        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        return hashString;
    }


}
