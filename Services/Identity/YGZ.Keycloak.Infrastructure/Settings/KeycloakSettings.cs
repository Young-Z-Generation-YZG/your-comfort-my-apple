

namespace YGZ.Keycloak.Infrastructure.Settings;

public class KeycloakSettings
{
    public const string SettingKey = "KeycloakSettings";
    public string Realm { get; set; } = default!;
    public string AuthServerUrl { get; set; } = default!;
    public NextjsClient NextjsClient { get; set; } = default!;
    public AdminClient AdminClient { get; set; } = default!;
}

public class NextjsClient
{
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
}

public class AdminClient
{
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
}