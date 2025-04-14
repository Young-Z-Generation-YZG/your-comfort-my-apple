
using Microsoft.Extensions.Options;
using YGZ.BuildingBlocks.Shared.Payments.Vnpay;
using YGZ.Ordering.Application.Abstractions.PaymentProviders.Vnpay;
using YGZ.Ordering.Infrastructure.Settings;

namespace YGZ.Ordering.Infrastructure.Payments.Vnpay;

public class VnpayProvider : IVnpayProvider
{
    private VnPayLibrary Vnpay = new VnPayLibrary();
    public string VnpHashSecret { get; private set; } = default!;

    public VnpayProvider(IOptions<VnpaySettings> vnpaySettings)
    {
        VnpHashSecret = vnpaySettings.Value.HashSecret;
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
