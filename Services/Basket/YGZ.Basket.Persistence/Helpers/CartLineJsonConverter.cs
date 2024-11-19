

using System.Text.Json;
using System.Text.Json.Serialization;
using YGZ.Basket.Domain.ShoppingCart.Entities;
using YGZ.Basket.Domain.ShoppingCart.ValueObjects;

namespace YGZ.Basket.Persistence.Helpers;

public class CartLineJsonConverter : JsonConverter<CartLine>
{
    public override CartLine? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var doc = JsonDocument.ParseValue(ref reader);
        var id = doc.RootElement.GetProperty("Id").GetProperty("Value").GetString();
        var cartLineId = CartLineId.ToEntity(Guid.Parse(id!));
        var productId = doc.RootElement.GetProperty("ProductId").GetString();
        var sku = doc.RootElement.GetProperty("Sku").GetString();
        var model = doc.RootElement.GetProperty("Model").GetString();
        var color = doc.RootElement.GetProperty("Color").GetString();
        var storage = doc.RootElement.GetProperty("Storage").GetString();
        var quantity = doc.RootElement.GetProperty("Quantity").GetInt32();
        var price = doc.RootElement.GetProperty("Price").GetDecimal();
        var imageUrl = doc.RootElement.GetProperty("Image_url").GetString();

        return new CartLine(cartLineId, productId!, sku!, model!, color!, storage!, quantity, price, imageUrl!);
    }

    public override void Write(Utf8JsonWriter writer, CartLine value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("ProductId", value.ProductId);
        writer.WriteString("Sku", value.Sku);
        writer.WriteString("Model", value.Model);
        writer.WriteString("Color", value.Color);
        writer.WriteString("Storage", value.Storage);
        writer.WriteNumber("Quantity", value.Quantity);
        writer.WriteNumber("Price", value.Price);
        writer.WriteString("ImageUrl", value.Image_url);
        writer.WriteEndObject();
    }
}
