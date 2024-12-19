
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Ordering.Domain.Orders;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;
using static YGZ.Ordering.Domain.Core.Enums.Enums;

namespace YGZ.Ordering.Persistence.Data.Configurations;

public class OrderConfiguaration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasConversion(orderId => orderId.Value, dbId => OrderId.Of(dbId));

        builder.Property(o => o.OrderCode).HasConversion(orderCode => orderCode.Value, dbCode => OrderCode.Of(dbCode));

        builder.Property(o => o.TotalAmount);

        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.UpdatedAt).IsRequired();

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();

        builder.HasMany(o => o.OrderLines)
            .WithOne()
            .HasForeignKey(ol => ol.OrderId);

        builder.ComplexProperty(o => o.ShippingAddress, addressbuilder =>
        {
            addressbuilder.IsRequired();
            addressbuilder.Property(a => a.ContactName).HasMaxLength(50).IsRequired(false);
            addressbuilder.Property(a => a.AddressLine).HasMaxLength(100).IsRequired(false);
            addressbuilder.Property(a => a.District).HasMaxLength(50).IsRequired(false);
            addressbuilder.Property(a => a.Province).HasMaxLength(50).IsRequired(false);
            addressbuilder.Property(a => a.Country).HasMaxLength(50).IsRequired(false);
            addressbuilder.Property(a => a.ContactEmail).HasMaxLength(50).IsRequired(false);
            addressbuilder.Property(a => a.ContactPhoneNumber).HasMaxLength(50).IsRequired(false);
        });


        builder.ComplexProperty(o => o.BillingAddress, addressbuilder =>
        {
            addressbuilder.IsRequired();
            addressbuilder.Property(a => a.ContactName).HasMaxLength(50).IsRequired(false);
            addressbuilder.Property(a => a.AddressLine).HasMaxLength(100).IsRequired(false);
            addressbuilder.Property(a => a.District).HasMaxLength(50).IsRequired(false);
            addressbuilder.Property(a => a.Province).HasMaxLength(50).IsRequired(false);
            addressbuilder.Property(a => a.Country).HasMaxLength(50).IsRequired(false);
            addressbuilder.Property(a => a.ContactEmail).HasMaxLength(50).IsRequired(false);
            addressbuilder.Property(a => a.ContactPhoneNumber).HasMaxLength(50).IsRequired(false);
        });


        builder.Property(o => o.Status).HasConversion(p => p.Value, p => OrderStatus.FromValue(p));
        
        builder.Property(o => o.PaymentType).HasConversion(p => p.Value, p => PaymentType.FromValue(p));
    }
}
