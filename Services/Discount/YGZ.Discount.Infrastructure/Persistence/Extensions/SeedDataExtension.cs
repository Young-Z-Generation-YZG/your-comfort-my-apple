
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Discount.Infrastructure.Persistence.Seeds;

namespace YGZ.Discount.Infrastructure.Persistence.Extensions;

public static class SeedDataExtension
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();

        await SeedAsync(context);
    }

    private static async Task SeedAsync(DiscountDbContext context)
    {
        await SeedCouponsAsync(context);
        await SeedEventsAsync(context);
        await SeedEventItemsAsync(context);
    }

    private static async Task SeedCouponsAsync(DiscountDbContext context)
    {
        if (!await context.Coupons.AnyAsync())
        {
            await context.Coupons.AddRangeAsync(SeedCouponData.Coupons);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedEventsAsync(DiscountDbContext context)
    {
        if (!await context.Events.AnyAsync())
        {
            await context.Events.AddRangeAsync(SeedEventData.Events);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedEventItemsAsync(DiscountDbContext context)
    {
        if (!await context.EventItems.AnyAsync())
        {
            await context.EventItems.AddRangeAsync(SeedEventItemData.EventItems);
            await context.SaveChangesAsync();
        }
    }
}
