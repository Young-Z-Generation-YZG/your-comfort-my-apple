
namespace YGZ.Ordering.Application.Builders;

public static class QueryParamBuilder
{
    public static Dictionary<string, string> Build<Type>(Type request)
    {
        var queryParams = new Dictionary<string, string>();

        // Get all properties of GetOrdersQuery
        var properties = typeof(Type).GetProperties();

        foreach (var property in properties)
        {
            var value = property.GetValue(request);

            if (value is not null)
            {
                string stringValue = value.ToString()!;

                if (!string.IsNullOrWhiteSpace(stringValue))
                {
                    string key = "_" + char.ToLower(property.Name[0]) + property.Name.Substring(1);

                    queryParams.Add(key, stringValue);
                }
            }
        }

        return queryParams;
    }
}
