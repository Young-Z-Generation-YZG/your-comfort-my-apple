using System.Text;
using System.Text.RegularExpressions;

namespace YGZ.BuildingBlocks.Shared.Utils;

public static class SnakeCaseSerializer
{
    public static string Serialize(string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return string.Empty;

        var provider = CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(provider);

        // Remove accents and convert to ASCII
        byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(str);
        var value = Encoding.ASCII.GetString(bytes);

        // Convert to lowercase
        value = value.ToLowerInvariant();

        // Remove invalid characters (keep only letters, numbers, spaces, and underscores)
        value = Regex.Replace(value, @"[^a-z0-9\s_]", "");

        // Convert multiple spaces/underscores into single underscores
        value = Regex.Replace(value, @"[\s_]+", "_");

        // Remove leading/trailing underscores
        value = value.Trim('_');

        // Convert camelCase and PascalCase to snake_case
        value = Regex.Replace(value, @"([a-z])([A-Z])", "$1_$2");

        // Convert spaces to underscores (in case any remain)
        value = value.Replace(" ", "_");

        // Remove multiple consecutive underscores
        value = Regex.Replace(value, @"_+", "_");

        return value;
    }
}
