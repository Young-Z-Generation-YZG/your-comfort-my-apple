﻿

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.Ordering.Infrastructure.Persistence.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using OrderDbContext context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await Task.CompletedTask;
    }
}
