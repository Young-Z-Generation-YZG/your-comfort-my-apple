
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.Catalog.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        return services;
    }
}
