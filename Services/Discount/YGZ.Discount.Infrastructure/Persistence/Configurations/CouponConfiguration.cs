using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Configurations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons");

        builder.HasKey(x => x.Id); // Define the primary key

        // Map CouponId's Id property to a column
        builder.Property(x => x.Id)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Value,
                   dbid => CouponId.Of(dbid)
               );

        builder.Property(x => x.Code)
               .HasConversion(
                   x => x.Value,
                   x => Code.Of(x)
               );

        // Configure SmartEnum properties with explicit conversion
        builder.Property(x => x.DiscountState)
               .HasConversion(
                   x => x.Name,
                   x => EDiscountState.FromName(x, false)
               )
               .HasColumnName("DiscountState");

        builder.Property(x => x.ProductClassification)
               .HasConversion(
                   x => x.Name,
                   x => EProductClassification.FromName(x, false)
               )
               .HasColumnName("ProductClassification");

        builder.Property(x => x.PromotionType)
               .HasConversion(
                   x => x.Name,
                   x => EPromotionType.FromName(x, false)
               )
               .HasColumnName("PromotionType");

        builder.Property(x => x.DiscountType)
               .HasConversion(
                   x => x.Name,
                   x => EDiscountType.FromName(x, false)
               )
               .HasColumnName("DiscountType");
    }
}