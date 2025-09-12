
namespace YGZ.Catalog.Domain.Common.ValueObjects;

public static class NormalizeString
{
    public static string Normalize(string name)
    {
        return name.Trim().Replace(" ", "").ToUpperInvariant();
    }
}
