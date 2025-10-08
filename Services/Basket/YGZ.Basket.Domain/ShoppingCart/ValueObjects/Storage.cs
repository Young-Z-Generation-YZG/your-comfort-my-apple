using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class Storage
{
    public string Name { get; init; }
    public string NormalizedName { get; init; }
    private Storage(string name, string normalizedName)
    {
        Name = name;
        NormalizedName = normalizedName;
    }
    public static Storage Create(string name)
    {
        var normalizedName = SnakeCaseSerializer.Serialize(name).ToUpper();

        EStorage.TryFromName(normalizedName, out var storageEnum);

        if (storageEnum is null)
        {
            throw new ArgumentException("Invalid storage", name);
        }

        return new Storage(name, normalizedName);
    }
}
