
using Microsoft.EntityFrameworkCore;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.PromotionEvent;
using YGZ.Discount.Domain.PromotionEvent.Entities;
using YGZ.Discount.Domain.PromotionItem;

namespace YGZ.Discount.Infrastructure.Persistence;

public class DiscountDbContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<PromotionItem> PromotionItems { get; set; }
    public DbSet<PromotionEvent> PromotionEvents { get; set; }
    public DbSet<PromotionGlobal> PromotionGlobals { get; set; }
    public DbSet<PromotionProduct> PromotionProducts { get; set; }
    public DbSet<PromotionCategory> PromotionCategories { get; set; }

    public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DiscountDbContext).Assembly);
    }
}
