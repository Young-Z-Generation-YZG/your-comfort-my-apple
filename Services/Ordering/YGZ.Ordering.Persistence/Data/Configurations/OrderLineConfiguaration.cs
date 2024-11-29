

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Persistence.Data.Configurations;

public class OrderLineConfiguaration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.HasKey(ol => ol.Id);
        builder.Property(ol => ol.Id).HasConversion(orderLineId => orderLineId.Value, dbId => OrderLineId.Of(dbId));

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(ol => ol.ProductId)
            .IsRequired();

        builder.Property(ol => ol.ProductName).HasMaxLength(50).IsRequired();
        builder.Property(ol => ol.ProductModel).HasMaxLength(50).IsRequired();
        builder.Property(ol => ol.ProductColor).HasMaxLength(50).IsRequired();
        builder.Property(ol => ol.ProductStorage).HasMaxLength(50).IsRequired();
        builder.Property(ol => ol.Quantity).IsRequired();
        builder.Property(ol => ol.Price).IsRequired();

    }
}
