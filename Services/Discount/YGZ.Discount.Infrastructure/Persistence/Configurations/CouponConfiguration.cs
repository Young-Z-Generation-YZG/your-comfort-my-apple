using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Core.Enums;
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

        //// Configure State property with conversion
        builder.Property(x => x.DiscountState) // Updated from Status to State
               .HasConversion(
                   x => x.Name,
                   x => DiscountState.FromName(x, false)
               )
               .HasColumnName("DiscountState");

        //// Configure Type property with conversion
        builder.Property(x => x.DiscountType)
               .HasConversion(
                   x => x.Name,
                   x => DiscountType.FromName(x, false)
               )
               .HasColumnName("DiscountType");

        //// Configure ProductNameTag property with conversion
        builder.Property(x => x.ProductNameTag)
               .HasConversion(
                   x => x.Name,
                   x => ProductNameTag.FromName(x, false)
               )
               .HasColumnName("ProductNameTag");

        builder.Property(x => x.PromotionEventType)
               .HasConversion(
                   x => x.Name,
                   x => PromotionEventType.FromName(x, false)
               )
               .HasColumnName("PromotionEventType");
    }
}