using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class Color
{
    public string Name { get; init; }
    public string NormalizedName { get; init; }
    private Color(string name, string normalizedName)
    {
        Name = name;
        NormalizedName = normalizedName;
    }

    public static Color Create(string name)
    {
        var normalizedName = SnakeCaseSerializer.Serialize(name).ToUpper();

        EColor.TryFromName(normalizedName, out var colorEum);

        if (colorEum is null)
        {
            throw new ArgumentException("Invalid color", name);
        }

        return new Color(name, normalizedName);
    }
}
