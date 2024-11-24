using Microsoft.EntityFrameworkCore;

namespace GYZ.Discount.Grpc.Data;

public static class MigrationExtension
{
    public static async Task<IApplicationBuilder> ApplyMigrationAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();

        // Await the migration to ensure it completes.
        await dbContext.Database.MigrateAsync();

        return app;
    }
}
