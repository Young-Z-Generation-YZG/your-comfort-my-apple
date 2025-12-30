using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Identity.Infrastructure.Persistence.Seeds;

namespace YGZ.Identity.Infrastructure.Persistence.Extensions;

public static class SeedDataExtension
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        var seedService = scope.ServiceProvider.GetRequiredService<SeedService>();

        await SeedAsync(context, seedService);
    }

    private static async Task SeedAsync(IdentityDbContext context, SeedService seedService)
    {
        await SeedRolesAsync(context);
        // await SeedUsersTenantYBZONEAsync(seedService);
        await SeedUsersAsync(context);
        await SeedUserRolesAsync(context);
    }

    private static async Task SeedUsersAsync(IdentityDbContext context)
    {
        if (!await context.Users.AnyAsync())
        {
            // await context.Users.AddRangeAsync(SeedData.Users);
            await context.Users.AddRangeAsync(SeedStaffs.UsersTenant_YBZONE);
            await context.Users.AddRangeAsync(SeedStaffs.UsersTenant_HCM_TD_KVC_1060);
            await context.Users.AddRangeAsync(SeedStaffs.UsersTenant_HCM_Q1_CMT8_92);
            await context.Users.AddRangeAsync(SeedStaffs.UsersTenant_HCM_Q9_LVV_123);
            await context.Users.AddRangeAsync(SeedCustomers.Customers);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedRolesAsync(IdentityDbContext context)
    {
        if (!await context.Roles.AnyAsync())
        {
            await context.Roles.AddRangeAsync(SeedData.Roles);
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedUserRolesAsync(IdentityDbContext context)
    {
        if (!await context.UserRoles.AnyAsync())
        {
            // await context.UserRoles.AddRangeAsync(SeedData.UserRoles);
            await context.UserRoles.AddRangeAsync(SeedStaffs.UserRoles_YBZONE);
            await context.UserRoles.AddRangeAsync(SeedStaffs.UserRoles_HCM_TD_KVC_1060);
            await context.UserRoles.AddRangeAsync(SeedStaffs.UserRoles_HCM_Q1_CMT8_92);
            await context.UserRoles.AddRangeAsync(SeedStaffs.UserRoles_HCM_Q9_LVV_123);
            await context.UserRoles.AddRangeAsync(SeedCustomers.UserRolesCustomer);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Seeds YBZONE tenant users using SeedService (follows RegisterHandler pattern)
    /// </summary>
    private static async Task SeedUsersTenantYBZONEAsync(SeedService seedService)
    {
        var result = await seedService.SeedUsersTenantYBZONEAsync();

        if (result.IsFailure)
        {
            // Log error but don't throw - allow other seed operations to continue
            // The SeedService already logs errors internally
        }
    }
}
