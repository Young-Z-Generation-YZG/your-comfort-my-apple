
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Core.Enums;
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
            .HasConversion(id => id.Value, value => OrderId.Of(value));

        builder.Property(o => o.LastModifiedBy)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => UserId.Of(value))
            .HasDefaultValue(null);

        //builder
        //    .Property(o => o.Code)
        //    .HasConversion(code => code.Value, value => Code.Of(value));
        builder.ComplexProperty(builder => builder.Code, code =>
        {
            code.Property(c => c.Value)
                .HasColumnName(nameof(Order.Code));
        });

        // Updated relationship configuration
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order) // Reference the navigation property if added, or leave empty with .WithOne()
            .HasForeignKey(oi => oi.OrderId); // Use the new OrderId property


        builder.ComplexProperty(builder => builder.ShippingAddress, address =>
        {
            address.Property(a => a.ContactName)
                .HasColumnName(nameof(Order.ShippingAddress) + nameof(Address.ContactName));

            address.Property(a => a.ContactEmail)
                .HasColumnName(nameof(Order.ShippingAddress) + nameof(Address.ContactEmail));

            address.Property(a => a.ContactPhoneNumber)
                .HasColumnName(nameof(Order.ShippingAddress) + nameof(Address.ContactPhoneNumber));

            address.Property(a => a.AddressLine)
                .HasColumnName(nameof(Order.ShippingAddress) + nameof(Address.AddressLine));

            address.Property(a => a.District)
                .HasColumnName(nameof(Order.ShippingAddress) + nameof(Address.District));

            address.Property(a => a.Province)
                .HasColumnName(nameof(Order.ShippingAddress) + nameof(Address.Province));

            address.Property(a => a.Country)
                .HasColumnName(nameof(Order.ShippingAddress) + nameof(Address.Country));
        });

        builder
            .Property(o => o.CustomerId)
            .HasConversion(id => id.Value, value => UserId.Of(value));

        builder.Property(x => x.Status) // Updated from Status to State
               .HasConversion(
                   x => x.Name,
                   x => OrderStatusEnum.FromName(x, false)
               )
               .HasDefaultValue(OrderStatusEnum.PENDING)
               .HasColumnName("Status");

        builder
            .Property(o => o.PaymentMethod)
            .HasConversion(
                   x => x.Name,
                   x => PaymentMethodEnum.FromName(x, false)
            )
            .HasColumnName("PaymentMethod")
            .IsRequired();
    }
}
