

using System.ComponentModel.DataAnnotations.Schema;
using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

[ComplexType]
public class Code : ValueObject
{
    public string Value { get; private set; } = default!;

    private Code(string value) => Value = value;

    public static Code Of(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        return new Code(code);
    }

    public static Code GenerateCode()
    {
        // Generate two random uppercase letters
        var random = new Random();

        // Generate 8 random digits
        int digits = random.Next(10000000, 100000000);

        return new Code($"#{digits:D8}");
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
