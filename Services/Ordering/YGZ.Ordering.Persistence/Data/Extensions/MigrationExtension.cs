

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.Ordering.Persistence.Data.Extensions;

public static class MigrationExtension
{
    public static async Task ApplyMigrationAsync(this WebApplication app) 
    {
        await Task.CompletedTask;

        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();
    }
}
