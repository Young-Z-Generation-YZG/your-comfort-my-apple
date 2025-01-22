using Microsoft.EntityFrameworkCore;
using YGZ.Identity.Infrastructure.Persistence;

namespace YGZ.Identity.Api.Common.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.Migrate();
    }
}
