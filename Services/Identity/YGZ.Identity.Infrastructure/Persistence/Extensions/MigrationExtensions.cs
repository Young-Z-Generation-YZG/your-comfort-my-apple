

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.Identity.Infrastructure.Persistence.Extensions;

public static class ApplyMigration
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using IdentityDbContext context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

        context.Database.Migrate();
    }
}
