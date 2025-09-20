

using YGZ.Discount.Domain.Abstractions;

namespace YGZ.Discount.Infrastructure.Utils;
public class UniqueCodeGenerator : IUniqueCodeGenerator
{
    public string GenerateUniqueCode()
    {
        // Generate two random uppercase letters
        var random = new Random();
        char letter1 = (char)random.Next('A', 'Z' + 1);
        char letter2 = (char)random.Next('A', 'Z' + 1);

        // Generate eight random digits
        int digits = random.Next(1000000, 10000000);

        // Combine letters and digits into a unique code
        return $"{letter1}{letter2}{digits:D7}";
    }
}
