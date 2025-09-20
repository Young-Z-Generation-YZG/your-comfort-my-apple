using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Event;
using YGZ.Discount.Domain.Event.Entities;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Configurations;

public class EventProductSKUConfiguration : IEntityTypeConfiguration<EventProductSKU>
{
    public void Configure(EntityTypeBuilder<EventProductSKU> builder)
    {
        builder.ToTable("EventProductSKUs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever().HasConversion(
            id => id.Value,
            dbid => EventProductSKUId.Of(dbid)
        );

        // Configure column order to match entity property order
        builder.Property(x => x.Id).HasColumnOrder(1);
        builder.Property(x => x.EventId).HasColumnOrder(2);
        builder.Property(x => x.SKUId).HasColumnOrder(3);
        builder.Property(x => x.ModelId).HasColumnOrder(4);
        builder.Property(x => x.ModelName).HasColumnOrder(5);
        builder.Property(x => x.StorageName).HasColumnOrder(6);
        builder.Property(x => x.ColorHaxCode).HasColumnOrder(7);
        builder.Property(x => x.ProductType).HasColumnOrder(8);
        builder.Property(x => x.ImageUrl).HasColumnOrder(9);
        builder.Property(x => x.DiscountType).HasColumnOrder(10);
        builder.Property(x => x.DiscountValue).HasColumnOrder(11);
        builder.Property(x => x.OriginalPrice).HasColumnOrder(12);
        builder.Property(x => x.Stock).HasColumnOrder(13);
        builder.Property(x => x.Sold).HasColumnOrder(14);
        builder.Property(x => x.CreatedAt).HasColumnOrder(15);
        builder.Property(x => x.UpdatedAt).HasColumnOrder(16);
        builder.Property(x => x.UpdatedBy).HasColumnOrder(17);
        builder.Property(x => x.IsDeleted).HasColumnOrder(18);
        builder.Property(x => x.DeletedAt).HasColumnOrder(19);
        builder.Property(x => x.DeletedBy).HasColumnOrder(20);

        // foreign key configuration - explicit relationship
        builder.HasOne<Event>()
               .WithMany(e => e.EventProductSKUs)
               .HasForeignKey(x => x.EventId)
               .IsRequired();

        // other property configurations
        builder.Property(x => x.ProductType)
               .HasConversion(
                   x => x.Name,
                   x => EProductType.FromName(x, false)
               )
               .HasColumnName("ProductType");

        builder.Property(x => x.DiscountType)
                .HasConversion(
                     x => x.Name,
                     x => EDiscountType.FromName(x, false)
                )
                .HasColumnName("DiscountType");
    }
}
