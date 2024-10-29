
using System.Text;
using System.Text.RegularExpressions;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Core.Common.ValueObjects;


public class Slug : ValueObject
{
    public string Value { get; }

    private Slug(string value)
    {
        Value = value;
    }

    public static Slug Create(string value)
    {
        var provider = System.Text.CodePagesEncodingProvider.Instance;

        Encoding.RegisterProvider(provider);

        // remove accents
        byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(value);
        var str = System.Text.Encoding.ASCII.GetString(bytes).ToLower();

        // invalid chars           
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

        // convert multiple spaces into one space   
        str = Regex.Replace(str, @"\s+", " ").Trim();

        // cut and trim 
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        str = Regex.Replace(str, @"\s", "-"); // hyphens   

        return new Slug(str);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
