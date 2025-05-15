
namespace YGZ.Catalog.Infrastructure.Settings;


public class CloudinarySettings
{
    public const string SettingKey = "CloudinarySettings";

    public string CloudName { get; set; } = default!;
    public string ApiKey { get; set; } = default!;
    public string ApiSecret { get; set; } = default!;
    public string FolderName { get; set; } = default!;
}
