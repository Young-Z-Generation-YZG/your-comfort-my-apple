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
            dbid => EventProducSKUId.Of(dbid)
        );

        // foreign key configuration
        builder.HasOne<Event>().WithMany().HasForeignKey(x => x.EventId).IsRequired();

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
