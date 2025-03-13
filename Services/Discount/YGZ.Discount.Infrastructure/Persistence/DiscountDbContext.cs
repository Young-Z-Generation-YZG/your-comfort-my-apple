
using Microsoft.EntityFrameworkCore;
using YGZ.Discount.Domain.Coupons;

namespace YGZ.Discount.Infrastructure.Persistence;

public class DiscountDbContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; }

    public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscountDbContext).Assembly);
    }
}
