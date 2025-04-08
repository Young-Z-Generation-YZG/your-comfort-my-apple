
namespace YGZ.Basket.Application.Abstractions.Providers.vnpay;

public class PaymentResponseModel
{
    public string OrderDescription { get; set; } = default!;
    public string TransactionId { get; set; } = default!;
    public string OrderId { get; set; } = default!;
    public string PaymentMethod { get; set; } = default!;
    public string PaymentId { get; set; } = default!;
    public bool Success { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string VnPayResponseCode { get; set; } = default!;
}