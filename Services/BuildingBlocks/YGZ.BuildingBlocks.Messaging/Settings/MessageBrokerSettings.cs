namespace YGZ.BuildingBlocks.Messaging.Settings;

public class MessageBrokerSettings
{
    public const string SettingKey = "MessageBrokerSettings";

    public string Host { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
}
