

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.TenantId)
            .ValueGeneratedNever()
            .HasConversion(
                id => id!.Value,
                value => TenantId.Of(value))
            .IsRequired(false);

        builder.Property(oi => oi.BranchId)
        .ValueGeneratedNever()
            .HasConversion(
                id => id!.Value,
                value => BranchId.Of(value))
            .IsRequired(false);

        builder
            .Property(oi => oi.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => OrderItemId.Of(value));

        builder.Property(oi => oi.OrderId)
            .HasConversion(
                id => id.Value,
                value => OrderId.Of(value))
            .IsRequired();

        builder.Property(oi => oi.SkuId)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(oi => oi.ModelId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(oi => oi.ModelName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(oi => oi.ColorName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(oi => oi.StorageName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(oi => oi.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(oi => oi.DisplayImageUrl)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(oi => oi.ModelSlug)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.SubTotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(oi => oi.DiscountAmount)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(oi => oi.TotalAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(oi => oi.IsReviewed)
            .HasDefaultValue(false);

        builder.Property(oi => oi.PromotionId)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(oi => oi.PromotionType)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(oi => oi.DiscountType)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(oi => oi.DiscountValue)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(oi => oi.UpdatedBy)
            .HasMaxLength(450);

        builder.Property(oi => oi.DeletedBy)
            .HasMaxLength(450);

        // builder.HasQueryFilter(oi => !oi.IsDeleted);
    }
}
