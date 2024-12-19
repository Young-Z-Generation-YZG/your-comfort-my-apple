

namespace YGZ.Ordering.Persistence.Data;

public sealed class ConnectionStrings
{
    public string Value { get; }

    public const string OrderingDb = "OrderingDb";

    public ConnectionStrings(string value)
    {
        Value = value;
    }

    public static implicit operator string(ConnectionStrings connectionString)
    {
        return connectionString.Value;
    }
}
