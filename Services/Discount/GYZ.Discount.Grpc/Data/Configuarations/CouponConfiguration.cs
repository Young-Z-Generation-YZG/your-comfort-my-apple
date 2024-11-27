using GYZ.Discount.Grpc.Common.Enums;
using GYZ.Discount.Grpc.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GYZ.Discount.Grpc.Data.Configuarations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.Property(p => p.Type)
            .HasConversion(
                p => p.Value,
                p => DiscountTypeEnum.FromValue(p));
    }
}
