using System.Text.Json.Serialization;
using Newtonsoft.Json;
using YGZ.BuildingBlocks.Shared.Contracts.ValueObjects;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.Basket.Domain.ShoppingCart.ValueObjects;

public class Model
{
    public string Name { get; init; }
    public string NormalizedName { get; init; }

    [System.Text.Json.Serialization.JsonConstructor]
    [Newtonsoft.Json.JsonConstructor]
    private Model(string name, string normalizedName)
    {
        Name = name;
        NormalizedName = normalizedName;
    }

    public static Model Create(string name)
    {
        var normalizedName = SnakeCaseSerializer.Serialize(name).ToUpper();

        EIphoneModel.TryFromName(normalizedName, out var iphoneModelEum);

        if (iphoneModelEum is null)
        {
            throw new ArgumentException("Invalid iphone model", name);
        }

        return new Model(name, normalizedName);
    }

    public ModelResponse ToResponse()
    {
        return new ModelResponse
        {
            Name = Name,
            NormalizedName = NormalizedName,    
            Order = 0
        };
    }
}
