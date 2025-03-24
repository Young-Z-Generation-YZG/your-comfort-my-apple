
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.PromotionEvent;
using YGZ.Discount.Domain.PromotionEvent.Entities;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Configurations;

public class PromotionGlobalConfiguration : IEntityTypeConfiguration<PromotionGlobal>
{
    public void Configure(EntityTypeBuilder<PromotionGlobal> builder)
    {
        builder.ToTable("PromotionGlobals");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                dbid => PromotionGlobalId.Of(dbid)
            );

        builder
            .Property(x => x.PromotionGlobalType)
            .HasConversion(x => x.Name, x => PromotionGlobalType.FromName(x, false))
            .HasColumnName("PromotionGlobalType");

        builder
            .HasOne(x => x.PromotionEvent)
            .WithMany()
            .HasForeignKey(x => x.PromotionEventId)
            .IsRequired();
    }
}
