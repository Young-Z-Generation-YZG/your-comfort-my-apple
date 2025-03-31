

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.Identity.Infrastructure.Persistence.Extensions;

public static class SeedDataExtensions
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        await SeedAsync(context);
    }

    private static async Task SeedAsync(IdentityDbContext context)
    {
        await SeedRolesAsync(context);
        await SeedUsersAsync(context);
        await SeedUserRolesAsync(context);
    }

    private static async Task SeedUsersAsync(IdentityDbContext context)
    {
        if (!await context.Users.AnyAsync())
        {
            await context.Users.AddRangeAsync(SeedData.Users);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedRolesAsync(IdentityDbContext context)
    {
        if (!await context.Roles.AnyAsync())
        {
            await context.Roles.AddRangeAsync(SeedData.Roles);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedUserRolesAsync(IdentityDbContext context)
    {
        if (!await context.UserRoles.AnyAsync())
        {
            await context.UserRoles.AddRangeAsync(SeedData.UserRoles);
            await context.SaveChangesAsync();
        }
    }
}
