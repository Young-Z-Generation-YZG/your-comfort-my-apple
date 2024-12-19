

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Ordering.Domain.Orders.Entities;
using YGZ.Ordering.Domain.Orders.ValueObjects;

namespace YGZ.Ordering.Persistence.Data.Configurations;

public class ProductConfiguaration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion(productId => productId.Value, dbId => ProductId.Of(dbId));

        builder.Property(p => p.Model).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Color).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Storage).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Price).IsRequired();
    }
}
