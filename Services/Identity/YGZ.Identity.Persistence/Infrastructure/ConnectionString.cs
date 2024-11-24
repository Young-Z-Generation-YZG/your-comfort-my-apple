
namespace YGZ.Identity.Persistence.Infrastructure;
public sealed class ConnectionString
{
    public string Value { get; }

    public const string SettingKey = "IdentityDb";

    public ConnectionString(string value)
    {
        Value = value;
    }

    public static implicit operator string(ConnectionString connectionString)
    {
        return connectionString.Value;
    }
}
