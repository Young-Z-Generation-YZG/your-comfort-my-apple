using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using YGZ.Basket.Application.Abstractions.Providers.Momo;
using YGZ.Basket.Infrastructure.Settings;

namespace YGZ.Basket.Infrastructure.Payments.Momo;

public class MomoProvider : IMomoProvider
{
    private readonly MomoSettings _momoSettings;
    private readonly WebClientSettings _webClientSettings;

    public MomoProvider(IOptions<MomoSettings> momoSettings, IOptions<WebClientSettings> webSettings)
    {
        _momoSettings = momoSettings.Value;
        _webClientSettings = webSettings.Value;
    }

    public async Task<MomoCreatePaymentResponseModel> CreatePaymentUrlAsync(MomoInformationModel model)
    {
        model.OrderId = DateTime.UtcNow.Ticks.ToString();
        model.OrderInfo = $"{model.FullName} {model.OrderId} {model.Amount}";

        var callBackUrl = $"{_webClientSettings.BaseUrl}/{_momoSettings.ReturnUrl}";

        var rawData = $"partnerCode={_momoSettings.PartnerCode}"
            + $"&accessKey={_momoSettings.AccessKey}"
            + $"&requestId={model.OrderId}"
            + $"&amount={model.Amount}"
            + $"&orderId={model.OrderId}"
            + $"&orderInfo={model.OrderInfo}"
            + $"&returnUrl={callBackUrl}"
            + $"&notifyUrl={_momoSettings.NotifyUrl}"
            + $"&extraData=";

        var signature = ComputeHmacSha256(rawData, _momoSettings.SecrectKey);

        var client = new RestClient(_momoSettings.MomoUrl);
        var request = new RestRequest() { Method = Method.Post };
        request.AddHeader("Content-Type", "application/json; charset=UTF-8");

        var requestBody = new
        {
            accessKey = _momoSettings.AccessKey,
            partnerCode = _momoSettings.PartnerCode,
            requestType = _momoSettings.RequestType,
            notifyUrl = _momoSettings.NotifyUrl,
            returnUrl = callBackUrl,
            orderId = model.OrderId,
            amount = model.Amount.ToString(),
            orderInfo = model.OrderInfo,
            requestId = model.OrderId,
            extraData = "",
            signature = signature,
        };

        request.AddParameter("application/json", JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);

        var response = await client.ExecuteAsync(request);

        return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content!)!;
    }

    public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collections)
    {
        var amount = collections.First(s => s.Key == "amount").Value;
        var orderInfo = collections.First(s => s.Key == "orderInfo").Value;
        var orderId = collections.First(s => s.Key == "orderId").Value;

        return new MomoExecuteResponseModel()
        {
            Amount = amount,
            OrderId = orderId,
            OrderInfo = orderInfo,
        };
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
