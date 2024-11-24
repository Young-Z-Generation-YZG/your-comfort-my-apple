

using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace YGZ.Basket.Persistence.Helpers;

public static class DependencyInjection
{
    public static IServiceCollection JsonConverterHelper(this IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Example configuration
                WriteIndented = true
            };

            options.Converters.Add(new CartLineJsonConverter()); // Add your custom converter
            return options;
        });


        return services;
    }
}
