

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

        await context.Orders.AddRangeAsync(OrderData_2025.Orders);
        await context.Orders.AddRangeAsync(OrderData_2024.Orders);
        await context.Orders.AddRangeAsync(OrderData_HCM_TD_KVC_1060.Orders);
        await context.Orders.AddRangeAsync(OrderData_HCM_Q1_CMT8_92.Orders);
        await context.Orders.AddRangeAsync(OrderData_HCM_Q9_LVV_33.Orders);

        await context.SaveChangesAsync();
    }
}
