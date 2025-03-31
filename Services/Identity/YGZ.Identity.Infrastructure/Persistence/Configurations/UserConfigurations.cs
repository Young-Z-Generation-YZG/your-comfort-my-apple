

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configure Image as an owned type
        builder.OwnsOne(u => u.Image, img =>
        {
            img.Property(i => i.ImageId)
               .HasColumnName("ImageId");
            img.Property(i => i.ImageUrl)
               .HasColumnName("ImageUrl");
        });
    }
}
