
namespace YGZ.Ordering.Application.Abstractions.PaymentProviders.Momo;

public class MomoExecuteResponseModel
{
    public string OrderId { get; set; } = default!;
    public string Amount { get; set; } = default!;
    public string OrderInfo { get; set; } = default!;
}
