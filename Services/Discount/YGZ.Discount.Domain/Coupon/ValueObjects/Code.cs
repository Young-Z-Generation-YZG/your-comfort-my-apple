

using YGZ.Discount.Domain.Core.Primitives;

namespace YGZ.Discount.Domain.Coupons.ValueObjects;

public class Code : ValueObject, IEquatable<Code>
{
    public string Value { get; private set; } = default!;

    public static Code Create()
    {
        // Generate two random uppercase letters
        var random = new Random();
        char letter1 = (char)random.Next('A', 'Z' + 1);
        char letter2 = (char)random.Next('A', 'Z' + 1);

        // Generate eight random digits
        int digits = random.Next(100000, 1000000);

        var code = $"{letter1}{letter2}{digits:D6}";

        return new Code { Value = code };
    }

    public static Code Of(string value)
    {
        return new Code { Value = value };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public bool Equals(Code? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Code);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
