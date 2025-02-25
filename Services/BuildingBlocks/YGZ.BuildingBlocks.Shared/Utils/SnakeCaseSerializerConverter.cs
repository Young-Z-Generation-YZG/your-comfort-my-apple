

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using YGZ.BuildingBlocks.Shared.Contracts.Auth;

namespace YGZ.BuildingBlocks.Shared.Utils;

public class SnakeCaseSerializerConverter : JsonConverter<object>
{
    public override bool CanConvert(Type typeToConvert)
    {
        // List types that should use this converter
        // return typeToConvert == typeof(LoginResponse); // Add more: || typeToConvert == typeof(RegisterResponse)
        return true;
    }

    public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonObject = JsonSerializer.Deserialize<JsonElement>(ref reader, options);
        var newOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize(jsonObject.GetRawText(), typeToConvert, newOptions)
            ?? throw new JsonException($"Failed to deserialize {typeToConvert.Name}");
    }

    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var property in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propName = ToSnakeCase(property.Name);
            var propValue = property.GetValue(value);

            writer.WritePropertyName(propName);
            JsonSerializer.Serialize(writer, propValue, property.PropertyType, options);
        }

        writer.WriteEndObject();
    }

    private static string ToSnakeCase(string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        var chars = new List<char> { char.ToLowerInvariant(str[0]) };
        for (int i = 1; i < str.Length; i++)
        {
            if (char.IsUpper(str[i]))
            {
                chars.Add('_');
                chars.Add(char.ToLowerInvariant(str[i]));
            }
            else
            {
                chars.Add(str[i]);
            }
        }
        return new string(chars.ToArray());
    }
}