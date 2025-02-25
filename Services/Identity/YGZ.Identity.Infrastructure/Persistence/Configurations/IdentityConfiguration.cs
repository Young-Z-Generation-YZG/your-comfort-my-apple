

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YGZ.Identity.Domain.Users;

namespace YGZ.Identity.Infrastructure.Persistence.Configurations;

public class IdentityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
    }

    private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        //builder.ToTable("Users");

        //builder.HasKey(x => x.Id);

        //builder.Property(x => x.Id)
        //    .ValueGeneratedNever()
        //    .HasConversion(
        //        id => id.Value.ToString(),
        //        value => UserId.CreateNew(Guid.Parse(value))
        //    );
    }
}
