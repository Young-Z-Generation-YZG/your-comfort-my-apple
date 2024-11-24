using GYZ.Discount.Grpc.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYZ.Discount.Grpc.Data;

public class DiscountDbContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; } = default!;
    public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(new Coupon { Id = 1, ProductName = "IPhone X", Description = "IPhone Discount", Amount = 100 });
        modelBuilder.Entity<Coupon>().HasData(new Coupon { Id = 2, ProductName = "Samsung 10", Description = "Samsung Discount", Amount = 50 });
        modelBuilder.Entity<Coupon>().HasData(new Coupon { Id = 3, ProductName = "Huawei P30", Description = "Huawei Discount", Amount = 75 });
    }
}
