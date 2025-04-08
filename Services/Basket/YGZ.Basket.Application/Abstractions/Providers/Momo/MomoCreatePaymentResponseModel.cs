
namespace YGZ.Basket.Application.Abstractions.Providers.Momo;

public class MomoCreatePaymentResponseModel
{
    public string RequestId { get; set; } = default!;
    public int ErrorCode { get; set; }
    public string OrderId { get; set; } = default!;
    public string Message { get; set; } = default!;
    public string LocalMessage { get; set; } = default!;
    public string RequestType { get; set; } = default!;
    public string PayUrl { get; set; } = default!;
    public string Signature { get; set; } = default!;
    public string QrCodeUrl { get; set; } = default!;
    public string Deeplink { get; set; } = default!;
    public string DeeplinkWebInApp { get; set; } = default!;
}
