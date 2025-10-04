using System.Text;
using System.Text.RegularExpressions;

namespace YGZ.BuildingBlocks.Shared.Utils;

public static class KebabCaseSerializer
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

        // Remove invalid characters (keep only letters, numbers, spaces, and hyphens)
        value = Regex.Replace(value, @"[^a-z0-9\s-]", "");

        // Convert multiple spaces/hyphens into single hyphens
        value = Regex.Replace(value, @"[\s-]+", "-");

        // Remove leading/trailing hyphens
        value = value.Trim('-');

        // Convert camelCase and PascalCase to kebab-case
        value = Regex.Replace(value, @"([a-z])([A-Z])", "$1-$2");

        // Convert spaces to hyphens (in case any remain)
        value = value.Replace(" ", "-");

        // Remove multiple consecutive hyphens
        value = Regex.Replace(value, @"-+", "-");

        return value;
    }
}
