using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Discount.Domain.Event;
using YGZ.Discount.Domain.Event.Entities;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Configurations;

public class EventItemConfiguration : IEntityTypeConfiguration<EventItem>
{
    public void Configure(EntityTypeBuilder<EventItem> builder)
    {
        builder.ToTable("EventItems");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever().HasConversion(
            id => id.Value,
            dbid => EventItemId.Of(dbid)
        );

        // Configure column order to match entity property order
        builder.Property(x => x.Id).HasColumnOrder(1);
        builder.Property(x => x.EventId).HasColumnOrder(2);
        builder.Property(x => x.SkuId).HasColumnOrder(3);
        builder.Property(x => x.TenantId).HasColumnOrder(4);
        builder.Property(x => x.BranchId).HasColumnOrder(5);
        builder.Property(x => x.ModelName).HasColumnOrder(6);
        builder.Property(x => x.NormalizedModel).HasColumnOrder(7);
        builder.Property(x => x.ColorName).HasColumnOrder(8);
        builder.Property(x => x.NormalizedColor).HasColumnOrder(9);
        builder.Property(x => x.StorageName).HasColumnOrder(11);
        builder.Property(x => x.NormalizedStorage).HasColumnOrder(12);
        builder.Property(x => x.ProductClassification).HasColumnOrder(13);
        builder.Property(x => x.ImageUrl).HasColumnOrder(14);
        builder.Property(x => x.DiscountType).HasColumnOrder(15);
        builder.Property(x => x.DiscountValue).HasColumnOrder(16);
        builder.Property(x => x.DiscountAmount).HasColumnOrder(17);
        builder.Property(x => x.OriginalPrice).HasColumnOrder(18);
        builder.Property(x => x.FinalPrice).HasColumnOrder(19);
        builder.Property(x => x.Stock).HasColumnOrder(20);
        builder.Property(x => x.Sold).HasColumnOrder(21);
        builder.Property(x => x.CreatedAt).HasColumnOrder(22);
        builder.Property(x => x.UpdatedAt).HasColumnOrder(23);
        builder.Property(x => x.UpdatedBy).HasColumnOrder(24);
        builder.Property(x => x.IsDeleted).HasColumnOrder(25);
        builder.Property(x => x.DeletedAt).HasColumnOrder(26);
        builder.Property(x => x.DeletedBy).HasColumnOrder(27);

        // foreign key configuration - explicit relationship
        builder.HasOne<Event>()
               .WithMany(e => e.EventItems)
               .HasForeignKey(x => x.EventId)
               .IsRequired();

        // other property configurations
        builder.Property(x => x.ProductClassification)
               .HasConversion(
                   x => x.Name,
                   x => EProductClassification.FromName(x, false)
               )
               .HasColumnName("ProductClassification");

        builder.Property(x => x.DiscountType)
                .HasConversion(
                     x => x.Name,
                     x => EDiscountType.FromName(x, false)
                )
                .HasColumnName("DiscountType");
    }
}
