

using YGZ.Ordering.Domain.Core.Primitives;

namespace YGZ.Ordering.Domain.Orders.ValueObjects;

public class OrderCode : ValueObject
{
    public string Value { get; } = default!;

    private OrderCode(string value)
    {
        Value = value;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public static OrderCode CreateNew()
    {
        return new OrderCode(GenerateOrderCode());
    }

    public static OrderCode Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Order code cannot be empty", nameof(value));
        }

        return new OrderCode(value);
    }

    private static string GenerateOrderCode()
    {
        // Generate two random uppercase letters
        var random = new Random();
        char letter1 = (char)random.Next('A', 'Z' + 1);
        char letter2 = (char)random.Next('A', 'Z' + 1);
        char letter3 = (char)random.Next('A', 'Z' + 1);
        char letter4 = (char)random.Next('A', 'Z' + 1);

        // Generate eight random digits
        int digits = random.Next(10000000, 100000000);

        // Combine letters and digits into a unique code
        return $"{letter1}{letter2}{letter3}{letter4}{digits:D8}";
    }
}
