

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Infrastructure.Persistence.Configurations;

public class ShippingAddressConfiguration : IEntityTypeConfiguration<ShippingAddress>
{
    public void Configure(EntityTypeBuilder<ShippingAddress> builder)
    {
        builder.HasKey(sa => sa.Id);

        builder.Property(sa => sa.Id)
               .ValueGeneratedNever()
               .HasConversion(id => id.Value, value => ShippingAddressId.Of(value));

        builder.OwnsOne(u => u.AddressDetail, ad =>
        {
            ad.Property(i => i.AddressLine).HasColumnName("AddressLine");
            ad.Property(i => i.AddressDistrict).HasColumnName("AddressDistrict");
            ad.Property(i => i.AddressProvince).HasColumnName("AddressProvince");
            ad.Property(i => i.AddressCountry).HasColumnName("AddressCountry");
        });
    }
}
 