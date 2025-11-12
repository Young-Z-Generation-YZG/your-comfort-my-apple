using System.Text.Json.Serialization;
using Newtonsoft.Json;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class Color
{
    public string Name { get; init; }
    public string NormalizedName { get; init; }
    
    [System.Text.Json.Serialization.JsonConstructor]
    [Newtonsoft.Json.JsonConstructor]
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

    public ColorResponse ToResponse()
    {
        return new ColorResponse
        {
            Name = Name,
            NormalizedName = NormalizedName,
            HexCode = string.Empty,
            ShowcaseImageId = string.Empty,
            Order = 0
        };
    }
}
