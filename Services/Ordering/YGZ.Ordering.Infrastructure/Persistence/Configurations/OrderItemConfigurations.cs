

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Configurations;

public class OrderItemConfigurations : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => OrderItemId.ToValueObject(value));

        // Add configuration for the new OrderId property
        builder.Property(oi => oi.OrderId)
            .HasConversion(id => id.Value, value => OrderId.ToValueObject(value));
    }
}
