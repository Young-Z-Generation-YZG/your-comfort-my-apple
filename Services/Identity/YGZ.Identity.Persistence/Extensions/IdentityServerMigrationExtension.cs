
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Identity.Persistence.Data;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using YGZ.Identity.Domain.Identity.Entities;
using YGZ.Identity.Domain.Core.Configs;
using Duende.IdentityServer.EntityFramework.Mappers;

namespace YGZ.Identity.Persistence.Extensions;

public class IdentityServerMigrationExtension
{
    public static async Task ApplyMigrationsAsync(WebApplication web)
    {
        using var scope = web.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();

        await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

        var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

        await context.Database.MigrateAsync();

        await EnsureSeedConfiguration(context, web.Configuration);

        await EnsureSeedData(
            scope.ServiceProvider.GetRequiredService<UserManager<User>>(), 
            scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>());
    }

    private static async Task EnsureSeedConfiguration(ConfigurationDbContext context, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(AppConfig)).Get<AppConfig>()!;

        foreach (var scope in IdentityServerResourceExtension.ApiScopes)
        {
            var exists = context.ApiScopes.Any(x => x.Name == scope.Name);

            if (!exists)
            {
                await context.ApiScopes.AddAsync(scope.ToEntity());
            }
        }

        foreach (var resource in IdentityServerResourceExtension.IdentityResources)
        {
            var exists = context.IdentityResources.Any(x => x.Name == resource.Name);

            if (!exists)
            {
                await context.IdentityResources.AddAsync(resource.ToEntity());
            }
        }

        foreach (var resource in IdentityServerResourceExtension.ApiResources)
        {
            var exists = context.ApiResources.Any(x => x.Name == resource.Name);

            if (!exists)
            {
                await context.ApiResources.AddAsync(resource.ToEntity());
            }
        }

        foreach (var client in IdentityServerResourceExtension.Clients(settings))
        {
            var exists = context.Clients.Any(x => x.ClientId == client.ClientId);

            if (!exists)
            {
                await context.Clients.AddAsync(client.ToEntity());
            }
        }

        await context.SaveChangesAsync();
    }

    private static async Task EnsureSeedData(UserManager<User> userManager, IPasswordHasher<User> passwordHasher)
    {
        if(userManager.FindByNameAsync("ygzadmin").Result == null)
        {
            var user = new User
            {
                UserName = "ygzadmin",
                Email = "ygzadmin@gmail.com",
                FirstName = "Admin",
                LastName = "User",
            };
            user.PasswordHash = passwordHasher.HashPassword(user, "123456789");

            var result = await userManager.CreateAsync(user).ConfigureAwait(false);

            if (result.Succeeded)
            {
                Console.WriteLine("Test User Created");
            }   

        }
    }
}
