
using Microsoft.Extensions.DependencyInjection;
using YGZ.Catalog.Domain.Core.Abstractions.Common;
using YGZ.Catalog.Infrastructure.Common;

namespace YGZ.Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}
