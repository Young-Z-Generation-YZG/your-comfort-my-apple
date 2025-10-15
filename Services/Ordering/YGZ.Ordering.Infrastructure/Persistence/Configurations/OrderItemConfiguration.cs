

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder
            .Property(oi => oi.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => OrderItemId.Of(value));

        builder.Property(oi => oi.OrderId)
            .HasConversion(id => id.Value, value => OrderId.Of(value))
            .IsRequired();

        builder.Property(oi => oi.SKUId)
            .HasConversion(id => id != null ? id.Value : null, value => value != null ? SkuId.Of(value) : null)
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

        builder.Property(oi => oi.IsReviewed)
            .HasDefaultValue(false);

        builder.ComplexProperty(oi => oi.Promotion, promotion =>
        {
            promotion.IsRequired();
            
            promotion.Property(p => p.PromotionIdOrCode)
                .HasColumnName("Promotion_IdOrCode")
                .HasMaxLength(100)
                .HasDefaultValue(null);

            promotion.Property(p => p.PromotionType)
                .HasColumnName("Promotion_Type")
                .HasMaxLength(50)
                .HasDefaultValue(null);

            promotion.Property(p => p.ProductUnitPrice)
                .HasColumnName("Promotion_ProductUnitPrice")
                .HasPrecision(18, 2)
                .HasDefaultValue(null);

            promotion.Property(p => p.DiscountType)
                .HasColumnName("Promotion_DiscountType")
                .HasMaxLength(50)
                .HasDefaultValue(null);

            promotion.Property(p => p.DiscountValue)
                .HasColumnName("Promotion_DiscountValue")
                .HasPrecision(18, 2)
                .HasDefaultValue(null);

            promotion.Property(p => p.DiscountAmount)
                .HasColumnName("Promotion_DiscountAmount")
                .HasPrecision(18, 2)
                .HasDefaultValue(null);
        });

        builder.Property(oi => oi.UpdatedBy)
            .HasMaxLength(450);

        builder.Property(oi => oi.DeletedBy)
            .HasMaxLength(450);

        // builder.HasQueryFilter(oi => !oi.IsDeleted);
    }
}
