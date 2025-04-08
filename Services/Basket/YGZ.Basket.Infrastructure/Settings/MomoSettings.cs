
namespace YGZ.Basket.Infrastructure.Settings;

public class MomoSettings
{
    public const string SettingKey = "MomoSettings";

    public string MomoUrl { get; set; } = default!;
    public string ReturnUrl { get; set; } = default!;
    public string NotifyUrl { get; set; } = default!;
    public string SecrectKey { get; set; } = default!;
    public string AccessKey { get; set; } = default!;
    public string PartnerCode { get; set; } = default!;
    public string RequestType { get; set; } = default!;
}
