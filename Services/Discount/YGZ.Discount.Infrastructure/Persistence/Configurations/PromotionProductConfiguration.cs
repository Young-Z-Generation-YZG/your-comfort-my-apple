
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.PromotionEvent.Entities;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Configurations;

public class PromotionProductConfiguration : IEntityTypeConfiguration<PromotionProduct>
{
    public void Configure(EntityTypeBuilder<PromotionProduct> builder)
    {
        builder.ToTable("PromotionProducts");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value,
                           dbid => ProductId.Of(dbid));

        builder
            .Property(x => x.PromotionGlobalId)
            .HasConversion(id => id.Value,
                           dbid => PromotionGlobalId.Of(dbid));

        builder.Property(x => x.DiscountType)
               .HasConversion(
                   x => x.Name,
                   x => DiscountType.FromName(x, false)
               )
               .HasColumnName("DiscountType");

        builder.HasOne<PromotionGlobal>().WithMany().HasForeignKey(x => x.PromotionGlobalId).IsRequired();
    }
}
