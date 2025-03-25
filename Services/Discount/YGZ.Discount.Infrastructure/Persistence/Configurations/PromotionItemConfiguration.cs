
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.PromotionEvent.ValueObjects;
using YGZ.Discount.Domain.PromotionItem;
using YGZ.Discount.Domain.PromotionItem.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Configurations;

public class PromotionItemConfiguration : IEntityTypeConfiguration<PromotionItem>
{
    public void Configure(EntityTypeBuilder<PromotionItem> builder)
    {
        builder.ToTable("PromotionItems");

        builder.HasKey(x => x.Id); // Define the primary key

        // Map PromotionItemId's Id property to a column
        builder.Property(x => x.Id)
               .ValueGeneratedNever()
               .HasConversion(
                   id => id.Value,
                   dbid => PromotionItemId.Of(dbid)
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

        builder.Property(x => x.EndDiscountType)
                .HasConversion(
                     x => x.Name,
                     x => EndDiscountType.FromName(x, false)
                )
                .HasColumnName("EndDiscountType");

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
