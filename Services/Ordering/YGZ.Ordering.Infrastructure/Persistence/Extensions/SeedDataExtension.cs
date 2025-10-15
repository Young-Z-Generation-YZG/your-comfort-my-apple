

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

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
        //await SeedOrdersAsync(context);
        //await SeedOrderItemsAsync(context);
    }

    //private static async Task SeedOrdersAsync(OrderDbContext context)
    //{
    //    if (!await context.Orders.AnyAsync())
    //    {
    //        await context.Orders.AddRangeAsync(SeedData.Orders);
    //        await context.SaveChangesAsync();
    //    }
    //}

    //private static async Task SeedOrderItemsAsync(OrderDbContext context)
    //{
    //    if (!await context.OrderItems.AnyAsync())
    //    {
    //        await context.OrderItems.AddRangeAsync(SeedData.OrderItems);
    //        await context.SaveChangesAsync();
    //    }
    //}
}
