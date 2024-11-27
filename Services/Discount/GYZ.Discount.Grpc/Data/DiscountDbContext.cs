using GYZ.Discount.Grpc.Common.Enums;
using GYZ.Discount.Grpc.Entities;
using Microsoft.EntityFrameworkCore;

namespace GYZ.Discount.Grpc.Data;

public class DiscountDbContext : DbContext
{
    public DbSet<Coupon> Coupons { get; set; } = default!;
    public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>()
            .Property(p => p.Type)
            .HasConversion(
                p => p.Value,
                p => DiscountTypeEnum.FromValue(p));

        modelBuilder.Entity<Coupon>()
            .Property(p => p.Status)
            .HasConversion(
                p => p.Value,
                p => DiscountStatusEnum.FromValue(p));

        modelBuilder.Entity<Coupon>().HasData(Coupon.CreateNew(
            "CODE",
            "Summer 2024",
            "Summer 2024 description",
            0.2,
            null,
            null,
            DateTime.UtcNow.ToUniversalTime(),
            DateTime.UtcNow.ToUniversalTime(),
            20
        ));

        //modelBuilder.Entity<Coupon>().HasData(new Coupon { Id = 1, ProductName = "IPhone X", Description = "IPhone Discount", Amount = 100 });
        //modelBuilder.Entity<Coupon>().HasData(new Coupon { Id = 2, ProductName = "Samsung 10", Description = "Samsung Discount", Amount = 50 });
        //modelBuilder.Entity<Coupon>().HasData(new Coupon { Id = 3, ProductName = "Huawei P30", Description = "Huawei Discount", Amount = 75 });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
    }
}
