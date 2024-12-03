

namespace YGZ.Basket.Infrastructure.Payments.Vnpay;

public class PaymentInformationModel
{
    public string OrderType { get; set; } = default!;
    public double Amount { get; set; } = default!;
    public string OrderDescription { get; set; } = default!;
    public string Name { get; set; } = default!;
}
