

namespace YGZ.Basket.Infrastructure.Settings;

public class VnpaySettings
{
    public const string SettingKey = "VnpaySettings";

    public string VnPayUrl { get; set; } = default!;
    public string ReturnUrl { get; set; } = default!;
    public string TmnCode { get; set; } = default!;
    public string HashSecret { get; set; } = default!;
    public string Command { get; set; } = default!;
    public string CurrCode { get; set; } = default!;
    public string Version { get; set; } = default!;
    public string Locale { get; set; } = default!;
    public string TimeZoneId { get; set; } = default!;
}
