
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Utils;

namespace YGZ.BuildingBlocks.Shared.Contracts.Baskets;

public class GetBasketResponse
{
    public List<CartItemResponse> Cart { get; set; } = new List<CartItemResponse>();
}

[JsonConverter(typeof(SnakeCaseSerializerConverter))]
public sealed record CartItemResponse(string ProductId,
                                      string ProductModel,
                                      string ProductColor,
                                      string ProductColorHex,
                                      int ProductStorage,
                                      decimal ProductPrice,
                                      string ProductImage,
                                      int Quantity) { }