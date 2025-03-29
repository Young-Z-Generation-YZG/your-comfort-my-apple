
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.PromotionEvent.Entities;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Configurations;

public class PromotionCategoryConfiguration : IEntityTypeConfiguration<PromotionCategory>
{
    public void Configure(EntityTypeBuilder<PromotionCategory> builder)
    {
        builder.ToTable("PromotionCategories");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                dbid => CategoryId.Of(dbid)
            );

        builder
            .Property(x => x.PromotionGlobalId)
            .HasConversion(
                id => id.Value,
                dbid => PromotionGlobalId.Of(dbid)
            );

        builder.Property(x => x.DiscountType)
               .HasConversion(
                   x => x.Name,
                   x => DiscountType.FromName(x, false)
               )
               .HasColumnName("DiscountType");

        builder.HasOne<PromotionGlobal>().WithMany().HasForeignKey(x => x.PromotionGlobalId).IsRequired();
    }
}
