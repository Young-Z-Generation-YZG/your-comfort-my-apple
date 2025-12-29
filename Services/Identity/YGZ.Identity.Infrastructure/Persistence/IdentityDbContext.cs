
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Infrastructure.Persistence;


public class IdentityDbContext : IdentityDbContext<User>
{
    private readonly ITenantHttpContext _tenantHttpContext;


    public IdentityDbContext(DbContextOptions<IdentityDbContext> options, ITenantHttpContext tenantHttpContext) : base(options)
    {
        _tenantHttpContext = tenantHttpContext;
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ShippingAddress> ShippingAddresses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName()!;

            if (tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }

        }

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Apply tenant filters dynamically - directly access _tenantHttpContext to ensure evaluation at query time
        builder.Entity<User>().HasQueryFilter(u => _tenantHttpContext.GetTenantId() == null || u.TenantId == _tenantHttpContext.GetTenantId());

        // Filter Profile entities through User relationship
        builder.Entity<Profile>().HasQueryFilter(p => _tenantHttpContext.GetTenantId() == null || (p.User != null && p.User.TenantId == _tenantHttpContext.GetTenantId()));

        // Filter ShippingAddress entities through User relationship
        builder.Entity<ShippingAddress>().HasQueryFilter(s => _tenantHttpContext.GetTenantId() == null || (s.User != null && s.User.TenantId == _tenantHttpContext.GetTenantId()));
    }
}
