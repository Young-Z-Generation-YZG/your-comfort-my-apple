

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Configurations;

public class PromotionEventConfiguration : IEntityTypeConfiguration<Domain.PromotionEvent.PromotionEvent>
{
    public void Configure(EntityTypeBuilder<Domain.PromotionEvent.PromotionEvent> builder)
    {
        builder.ToTable("PromotionEvents");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever().HasConversion(
            id => id.Value,
            dbid => PromotionEventId.Of(dbid)
        );

        builder.Property(x => x.DiscountState) // Updated from Status to State
               .HasConversion(
                   x => x.Name,
                   x => DiscountState.FromName(x, false)
               )
               .HasColumnName("DiscountState");

        builder.Property(x => x.PromotionEventType)
               .HasConversion(
                   x => x.Name,
                   x => PromotionEventType.FromName(x, false)
               )
               .HasColumnName("PromotionEventType");

    }
}
