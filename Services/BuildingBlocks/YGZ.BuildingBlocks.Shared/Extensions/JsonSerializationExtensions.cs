
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace YGZ.BuildingBlocks.Shared.Extensions;

public static class JsonSerializationExtensions
{
    private static readonly SnakeCaseNamingStrategy _snakeCaseNamingStrategy
        = new SnakeCaseNamingStrategy();

    private static readonly JsonSerializerSettings _snakeCaseSettings = new JsonSerializerSettings
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = _snakeCaseNamingStrategy
        }
    };

    public static string ToSnakeCase<T>(this T instance)
    {
        if (instance == null)
        {
            throw new ArgumentNullException(paramName: nameof(instance));
        }

        return JsonConvert.SerializeObject(instance, _snakeCaseSettings);
    }

    public static string ToSnakeCase(this string @string)
    {
        if (@string == null)
        {
            throw new ArgumentNullException(paramName: nameof(@string));
        }

        return _snakeCaseNamingStrategy.GetPropertyName(@string, false);
    }
}