

namespace YGZ.Basket.Persistence.Infrastructure;

public sealed class ConnectionString
{
    public string Value { get; }

    public const string BasketDb = "BasketDb";

    public ConnectionString(string value)
    {
        Value = value;
    }

    public static implicit operator string(ConnectionString connectionString)
    {
        return connectionString.Value;
    }
}
