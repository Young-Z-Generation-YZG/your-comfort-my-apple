using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Coupons;
using YGZ.Discount.Domain.Coupons.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Configurations;

public class CouponConfigurations : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons");

        builder.HasKey(x => x.Id); // Define the primary key

        // Map CouponId's Id property to a column
        builder.Property(x => x.Id)
               .ValueGeneratedNever()
               .HasConversion(
                   code => code.Value,
                   code => Code.Create(code)
               );

        //// Configure State property with conversion
        builder.Property(x => x.State) // Updated from Status to State
               .HasConversion(
                   x => x.Name,
                   x => DiscountStateEnum.FromName(x, false)
               )
               .HasColumnName("State");

        //// Configure Type property with conversion
        builder.Property(x => x.Type)
               .HasConversion(
                   x => x.Name,
                   x => DiscountTypeEnum.FromName(x, false)
               )
               .HasColumnName("Type");

        //builder.OwnsOne(x => x.State);
        //builder.OwnsOne(x => x.Type);
    }
}