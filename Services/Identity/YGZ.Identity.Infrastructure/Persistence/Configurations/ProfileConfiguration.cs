
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Identity.Domain.Core.Enums;
using YGZ.Identity.Domain.Users.Entities;
using YGZ.Identity.Domain.Users.ValueObjects;

namespace YGZ.Identity.Infrastructure.Persistence.Configurations;

public class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .HasConversion(id => id.Value, value => ProfileId.Of(value));

        builder.OwnsOne(u => u.Image, img =>
        {
            img.Property(i => i.ImageId)
               .HasDefaultValue(string.Empty)
               .HasColumnName("ImageId");

            img.Property(i => i.ImageUrl)
               .HasDefaultValue(string.Empty)
               .HasColumnName("ImageUrl");
        });

        builder.Property(x => x.Gender) // Updated from Status to State
               .HasConversion(
                   x => x.Name,
                   x => Gender.FromName(x, false)
               )
               .HasColumnName("Gender");
    }
}
