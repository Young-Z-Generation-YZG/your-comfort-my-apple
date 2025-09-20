using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Discount.Domain.Core.Enums;
using YGZ.Discount.Domain.Event;
using YGZ.Discount.Domain.Event.Entities;
using YGZ.Discount.Domain.Event.ValueObjects;

namespace YGZ.Discount.Infrastructure.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever().HasConversion(
            id => id.Value,
            dbid => EventId.Of(dbid)
        );

        // Configure column order to match entity property order
        builder.Property(x => x.Id).HasColumnOrder(1);
        builder.Property(x => x.Title).HasColumnOrder(2);
        builder.Property(x => x.Description).HasColumnOrder(3);
        builder.Property(x => x.State).HasColumnOrder(4);
        builder.Property(x => x.StartDate).HasColumnOrder(5);
        builder.Property(x => x.EndDate).HasColumnOrder(6);
        builder.Property(x => x.CreatedAt).HasColumnOrder(7);
        builder.Property(x => x.UpdatedAt).HasColumnOrder(8);
        builder.Property(x => x.UpdatedBy).HasColumnOrder(9);
        builder.Property(x => x.IsDeleted).HasColumnOrder(10);
        builder.Property(x => x.DeletedAt).HasColumnOrder(11);
        builder.Property(x => x.DeletedBy).HasColumnOrder(12);

        builder.Property(x => x.State) // Updated from Status to State
               .HasConversion(
                   x => x.Name,
                   x => EState.FromName(x, false)
               )
               .HasColumnName("State");

        // Configure the relationship with EventProductSKUs using the navigation property
        builder.HasMany(e => e.EventProductSKUs)
               .WithOne()
               .HasForeignKey(ep => ep.EventId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
