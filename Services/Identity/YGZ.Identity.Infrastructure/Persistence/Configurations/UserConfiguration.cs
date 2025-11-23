

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Domain.Users.Entities;

namespace YGZ.Identity.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.TenantId).IsUnique(false);
        builder.HasIndex(u => u.BranchId).IsUnique(false);

        builder.Property(u => u.TenantId)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(u => u.BranchId)
            .HasMaxLength(255)
            .IsRequired(false);

        builder.HasOne(u => u.Profile)
            .WithOne(p => p.User)
            .HasForeignKey<Profile>(p => p.UserId)
            .IsRequired();

        builder.HasMany(u => u.ShippingAddresses)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .IsRequired();
    }
}
