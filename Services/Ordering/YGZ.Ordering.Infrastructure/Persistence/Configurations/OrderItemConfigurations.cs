

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Configurations;

public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);
        builder
            .Property(oi => oi.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => OrderItemId.Of(value));

        // Add configuration for the new OrderId property
        builder.Property(oi => oi.OrderId)
            .HasConversion(id => id.Value, value => OrderId.Of(value));

        builder.OwnsOne(oi => oi.Promotion, promotionBuilder =>
        {
            promotionBuilder.Property(a => a.PromotionIdOrCode).HasColumnName(nameof(Promotion.PromotionIdOrCode));
            promotionBuilder.Property(a => a.PromotionTitle).HasColumnName(nameof(Promotion.PromotionTitle));
            promotionBuilder.Property(a => a.PromotionEventType).HasColumnName(nameof(Promotion.PromotionEventType));
            promotionBuilder.Property(a => a.PromotionDiscountType).HasColumnName(nameof(Promotion.PromotionDiscountType));
            promotionBuilder.Property(a => a.PromotionDiscountValue).HasColumnName(nameof(Promotion.PromotionDiscountValue));
            promotionBuilder.Property(a => a.PromotionDiscountUnitPrice).HasColumnName(nameof(Promotion.PromotionDiscountUnitPrice));
            promotionBuilder.Property(a => a.PromotionAppliedProductCount).HasColumnName(nameof(Promotion.PromotionAppliedProductCount));
            promotionBuilder.Property(a => a.PromotionFinalPrice).HasColumnName(nameof(Promotion.PromotionFinalPrice));
        });

        builder.Navigation(oi => oi.Promotion).IsRequired(false); // Mark as optional
    }
}
