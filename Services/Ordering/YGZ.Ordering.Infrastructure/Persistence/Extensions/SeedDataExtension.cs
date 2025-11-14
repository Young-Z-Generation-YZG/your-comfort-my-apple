

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Ordering.Infrastructure.Persistence.Seeds;

namespace YGZ.Ordering.Infrastructure.Persistence.Extensions;

public static class SeedDataExtension
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

        await SeedAsync(context);
    }

    private static async Task SeedAsync(OrderDbContext context)
    {
        await SeedOrdersAsync(context);
    }

    private static async Task SeedOrdersAsync(OrderDbContext context)
    {
        if (await context.Orders.AnyAsync())
        {
            return;
        }

        await context.Orders.AddRangeAsync(SeedOrderData.Orders);
        await context.SaveChangesAsync();
    }
}
