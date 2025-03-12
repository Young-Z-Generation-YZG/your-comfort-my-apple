
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Enums;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Configurations;

public class OrderConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder
            .Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => OrderId.ToValueObject(value));

        // Updated relationship configuration
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order) // Reference the navigation property if added, or leave empty with .WithOne()
            .HasForeignKey(oi => oi.OrderId) // Use the new OrderId property
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne<Address>()           // The related entity is Address
            .WithMany()                  // Address has no navigation property back to Order
            .HasForeignKey(o => o.ShippingAddressId); // ShippingAddress is the foreign key

        builder
            .Property(o => o.ShippingAddressId)
            .HasConversion(
                addressId => addressId.Value,          // Convert AddressId to Guid for storage
                value => AddressId.ToValueObject(value) // Convert Guid back to AddressId when reading
            );

        builder
            .Property(o => o.Code)
            .HasConversion(code => code.Value, value => Code.Create(value));

        builder
            .Property(o => o.CustomerId)
            .HasConversion(id => id.Value, value => UserId.ToValueObject(value));

        builder.Property(x => x.Status) // Updated from Status to State
               .HasConversion(
                   x => x.Name,
                   x => OrderStatusEnum.FromName(x, false)
               )
               .HasColumnName("Status");

        builder
            .Property(o => o.PaymentType)
            .HasConversion(
                   x => x.Name,
                   x => PaymentTypeEnum.FromName(x, false)
            )
            .HasColumnName("PaymentType");
    }
}
