

using Microsoft.AspNetCore.Http;

namespace YGZ.Ordering.Application.Abstractions.PaymentProviders.Momo;

public interface IMomoProvider
{
    bool IpnCheck(Dictionary<string, string> momoQueryParams);
}
