
namespace YGZ.Ordering.Infrastructure.Settings;

public class VnpaySettings
{
    public const string SettingKey = "VnpaySettings";

    public string HashSecret { get; set; } = default!;
}
