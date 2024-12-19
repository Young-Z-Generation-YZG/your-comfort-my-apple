
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Persistence.Data.Configurations;

internal class CustomerConfiguaration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id).HasConversion(customerId => customerId.Value, dbId => CustomerId.Of(dbId));

        builder.Property(c => c.Name).HasMaxLength(50).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(50).IsRequired();

        builder.HasIndex(c => c.Email).IsUnique();
    }
}

