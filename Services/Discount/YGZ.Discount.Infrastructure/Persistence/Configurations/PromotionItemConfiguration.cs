
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

        builder.Property(x => x.ProductId)
                .HasConversion(
                     id => id.Value,
                     dbid => ProductId.Of(dbid)
                );

        //// Configure State property with conversion
        builder.Property(x => x.State) // Updated from Status to State
               .HasConversion(
                   x => x.Name,
                   x => DiscountState.FromName(x, false)
               )
               .HasColumnName("State");

        //// Configure Type property with conversion
        builder.Property(x => x.Type)
               .HasConversion(
                   x => x.Name,
                   x => DiscountType.FromName(x, false)
               )
               .HasColumnName("Type");

        //// Configure ProductNameTag property with conversion
        builder.Property(x => x.NameTag)
               .HasConversion(
                   x => x.Name,
                   x => NameTag.FromName(x, false)
               )
               .HasColumnName("NameTag");
    }
}
