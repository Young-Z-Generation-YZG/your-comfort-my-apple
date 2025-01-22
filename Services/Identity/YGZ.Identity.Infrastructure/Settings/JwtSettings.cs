
namespace YGZ.Identity.Infrastructure.Settings;

public class JwtSettings
{
    public const string SettingKey = "JwtSettings";
    public string SecretKey { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string MetadataAddress { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string ValidAudience { get; set; } = string.Empty;
    public double ExpiredSeconds { get; set; }
}
