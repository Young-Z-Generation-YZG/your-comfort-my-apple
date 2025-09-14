
using System.Text.RegularExpressions;
using System.Text;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Slug : ValueObject
{
    public string Value { get; set; } = string.Empty;

    public static Slug Create(string value)
    {
        var provider = CodePagesEncodingProvider.Instance;

        Encoding.RegisterProvider(provider);

        // remove accents
        byte[] bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
        var str = Encoding.ASCII.GetString(bytes).ToLower();

        // invalid chars           
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

        // convert multiple spaces into one space   
        str = Regex.Replace(str, @"\s+", " ").Trim();

        // cut and trim 
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        str = Regex.Replace(str, @"\s", "-"); // hyphens   

        return new Slug
        {
            Value = str
        };
    }

    public static Slug Of(string value) => Create(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
