

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.Keycloak.Infrastructure.Persistence.Extensions;

public static class SeedDataExtensions
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await SeedAsync(context);
    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedUsersAsync(context);
    }

    private static async Task SeedUsersAsync(ApplicationDbContext context)
    {
        if(!await context.Users.AnyAsync())
        {
            await context.Users.AddRangeAsync(SeedData.Users);
            await context.SaveChangesAsync();
        }
    }
}
