
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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
        await SeedPromotionCouponsAsync(context);
        //await SeedPromotionEventsAsync(context);
        //await SeedPromotionGlobalsAsync(context);
        //await SeedPromotionProductsAsync(context);
        //await SeedPromotionCategoriesAsync(context);
        //await SeedPromotionItemsAsync(context);
    }

    //private static async Task SeedPromotionItemsAsync(DiscountDbContext context)
    //{
    //    if (!await context.PromotionItems.AnyAsync())
    //    {
    //        await context.PromotionItems.AddRangeAsync(SeedData.PromotionItems);
    //        await context.SaveChangesAsync();
    //    }
    //}

    private static async Task SeedPromotionCouponsAsync(DiscountDbContext context)
    {
        if (!await context.Coupons.AnyAsync())
        {
            await context.Coupons.AddRangeAsync(SeedData.Coupons);
            await context.SaveChangesAsync();
        }
    }

    //private static async Task SeedPromotionCategoriesAsync(DiscountDbContext context)
    //{
    //    if (!await context.PromotionCategories.AnyAsync())
    //    {
    //        await context.PromotionCategories.AddRangeAsync(SeedData.promotionCategories);
    //        await context.SaveChangesAsync();
    //    }
    //}

    //private static async Task SeedPromotionProductsAsync(DiscountDbContext context)
    //{
    //    if (!await context.PromotionProducts.AnyAsync())
    //    {
    //        await context.PromotionProducts.AddRangeAsync(SeedData.PromotionProducts);
    //        await context.SaveChangesAsync();
    //    }
    //}

    //private static async Task SeedPromotionEventsAsync(DiscountDbContext context)
    //{
    //    if (!await context.PromotionEvents.AnyAsync())
    //    {
    //        await context.PromotionEvents.AddRangeAsync(SeedData.PromotionEvents);
    //        await context.SaveChangesAsync();
    //    }
    //}

    //private static async Task SeedPromotionGlobalsAsync(DiscountDbContext context)
    //{
    //    if (!await context.PromotionGlobals.AnyAsync())
    //    {
    //        await context.PromotionGlobals.AddRangeAsync(SeedData.PromotionGlobals);
    //        await context.SaveChangesAsync();
    //    }
    //}
}
