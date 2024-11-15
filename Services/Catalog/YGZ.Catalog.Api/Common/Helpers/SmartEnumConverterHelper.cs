using Ardalis.SmartEnum;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YGZ.Catalog.Api.Common.Helpers;


public class SmartEnumConverterHelper<TEnum> : JsonConverter<TEnum> where TEnum : SmartEnum<TEnum>
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt32();
        return SmartEnum<TEnum>.FromValue(value);
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("Name", value.Name);
        writer.WriteNumber("Value", value.Value);
        writer.WriteEndObject();
    }
}
