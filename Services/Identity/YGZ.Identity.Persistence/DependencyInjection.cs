﻿

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Identity.Application.Identity.Common.Abstractions;
using YGZ.Identity.Persistence.Extensions;
using YGZ.Identity.Persistence.Infrastructure;
using YGZ.Identity.Persistence.Services;

namespace YGZ.Identity.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionString.SettingKey)!;

        services.AddSingleton(new ConnectionString(connectionString));

        services.AddIdentityExtension(configuration);

        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}