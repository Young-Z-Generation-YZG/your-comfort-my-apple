
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YGZ.BuildingBlocks.Shared.Abstractions.HttpContext;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Infrastructure.Persistence;


public class IdentityDbContext : IdentityDbContext<User>
{
    private readonly ILogger<IdentityDbContext> _logger;
    private readonly ITenantHttpContext _tenantHttpContext;
    private readonly IUserHttpContext _userHttpContext;
    private readonly IList<string> _userRoles = new List<string>();


    public IdentityDbContext(DbContextOptions<IdentityDbContext> options, ITenantHttpContext tenantHttpContext, IUserHttpContext userHttpContext, ILogger<IdentityDbContext> logger) : base(options)
    {
        _tenantHttpContext = tenantHttpContext;
        _userHttpContext = userHttpContext;
        _userRoles = userHttpContext.GetUserRoles();
        _logger = logger;
    }

    public DbSet<Profile> Profiles { get; set; }
    public DbSet<ShippingAddress> ShippingAddresses { get; set; }

    // Property to get current tenant ID dynamically
    private string? CurrentTenantId => _tenantHttpContext.GetTenantId();

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
