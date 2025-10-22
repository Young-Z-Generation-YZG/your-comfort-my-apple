
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.BuildingBlocks.Shared.ValueObjects;
using YGZ.Ordering.Application.Orders;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder
            .Property(o => o.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => OrderId.Of(value));

        builder
            .Property(o => o.TenantId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? TenantId.Of(value.Value) : null)
            .IsRequired(false);

        builder
            .Property(o => o.BranchId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? BranchId.Of(value.Value) : null)
            .IsRequired(false);

        builder
            .Property(o => o.CustomerId)
            .HasConversion(id => id.Value, value => UserId.Of(value.ToString()))
            .IsRequired();

        builder.ComplexProperty(o => o.Code, code =>
        {
            code.Property(c => c.Value)
                .HasColumnName(nameof(Order.Code))
                .IsRequired();
        });

        builder.Property(o => o.OrderStatus)
            .HasConversion(
                status => status.Name,
                name => EOrderStatus.FromName(name, false))
            .HasColumnName("OrderStatus")
            .IsRequired();

        builder.Property(o => o.PaymentMethod)
            .HasConversion(
                method => method.Name,
                name => EPaymentMethod.FromName(name, false))
            .HasColumnName("PaymentMethod")
            .IsRequired();

        builder.ComplexProperty(o => o.ShippingAddress, address =>
        {
            address.Property(a => a.ContactName)
                .HasColumnName("ShippingAddress_" + nameof(ShippingAddress.ContactName))
                .IsRequired();

            address.Property(a => a.ContactEmail)
                .HasColumnName("ShippingAddress_" + nameof(ShippingAddress.ContactEmail))
                .IsRequired();

            address.Property(a => a.ContactPhoneNumber)
                .HasColumnName("ShippingAddress_" + nameof(ShippingAddress.ContactPhoneNumber))
                .IsRequired();

            address.Property(a => a.AddressLine)
                .HasColumnName("ShippingAddress_" + nameof(ShippingAddress.AddressLine))
                .IsRequired();

            address.Property(a => a.District)
                .HasColumnName("ShippingAddress_" + nameof(ShippingAddress.District))
                .IsRequired();

            address.Property(a => a.Province)
                .HasColumnName("ShippingAddress_" + nameof(ShippingAddress.Province))
                .IsRequired();

            address.Property(a => a.Country)
                .HasColumnName("ShippingAddress_" + nameof(ShippingAddress.Country))
                .IsRequired();
        });

        builder.Property(o => o.PromotionId)
            .HasMaxLength(450)
            .IsRequired(false);

        builder.Property(o => o.PromotionType)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(o => o.DiscountType)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(o => o.DiscountValue)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(o => o.DiscountAmount)
            .HasPrecision(18, 2)
            .IsRequired(false);

        var navigation = builder.Metadata.FindNavigation(nameof(Order.OrderItems));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(o => o.UpdatedBy)
            .HasMaxLength(450);

        builder.Property(o => o.DeletedBy)
            .HasMaxLength(450);

        builder.HasIndex(o => o.TenantId);
        builder.HasIndex(o => o.BranchId);
    }
}
