
namespace YGZ.Catalog.Infrastructure.Settings;

public class OpenRouterSettings
{
    public const string SettingKey = "OpenRouterSettings";

    public string ApiKey { get; set; } = default!;
    public string Model { get; set; } = "openai/gpt-4o-mini";
}
