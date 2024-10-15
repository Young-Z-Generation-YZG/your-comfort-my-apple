using Microsoft.EntityFrameworkCore;
using YGZ.Identity.Persistence.Data;

namespace YGZ.Identity.Api.Common.Extensions
{
    public static class MigrationExtention
    {
        public static void AppMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();
        }
    }
}
