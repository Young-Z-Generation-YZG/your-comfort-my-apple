

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
        var productId = doc.RootElement.GetProperty("ProductItemId").GetString();
        var model = doc.RootElement.GetProperty("Model").GetString();
        var color = doc.RootElement.GetProperty("Color").GetString();
        var storage = doc.RootElement.GetProperty("Storage").GetInt32();
        var imageUrl = doc.RootElement.GetProperty("PrimaryImageUrl").GetString();
        var price = doc.RootElement.GetProperty("Price").GetDecimal();
        var quantity = doc.RootElement.GetProperty("Quantity").GetInt32();
        var discountAmount = doc.RootElement.GetProperty("DiscountAmount").GetDecimal();

        return new CartLine(cartLineId, productId!, model!, color!, storage!, imageUrl!, quantity, price, discountAmount);
    }

    public override void Write(Utf8JsonWriter writer, CartLine value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("ProductItemId", value.ProductItemId);
        writer.WriteString("Model", value.Model);
        writer.WriteString("Color", value.Color);
        writer.WriteNumber("Storage", value.Storage);
        writer.WriteString("PrimaryImageUrl", value.PrimaryImageUrl);
        writer.WriteNumber("Quantity", value.Quantity);
        writer.WriteNumber("Price", value.Price);
        writer.WriteNumber("DiscountAmount", value.DiscountAmount);
        writer.WriteNumber("SubTotal", value.SubTotal);

        writer.WriteEndObject();
    }
}
