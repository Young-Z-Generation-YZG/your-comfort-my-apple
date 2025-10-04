using YGZ.BuildingBlocks.Shared.Utils;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Slug : ValueObject
{
    public string Value { get; set; } = string.Empty;

    public static Slug Create(string str)
    {
        var value = KebabCaseSerializer.Serialize(str);

        return new Slug
        {
            Value = value
        };
    }

    public static Slug Of(string value) => Create(value);

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
