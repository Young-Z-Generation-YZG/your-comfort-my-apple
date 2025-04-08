

namespace YGZ.Basket.Application.Abstractions.Providers.vnpay;

public class VnpayInformationModel
{
    public string Name { get; set; } = default!;
    public string OrderType { get; set; } = default!;
    public string OrderDescription { get; set; } = default!;
    public decimal Amount { get; set; } = default!;
}
