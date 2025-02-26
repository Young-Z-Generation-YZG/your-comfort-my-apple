

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using YGZ.Keycloak.Domain.Users;

//namespace YGZ.Keycloak.Infrastructure.Persistence.Configurations;

//public class UserConfigurations : IEntityTypeConfiguration<User>
//{
//    public void Configure(EntityTypeBuilder<User> builder)
//    {
//        ConfigureUsersTable(builder);
//    }

//    private void ConfigureUsersTable(EntityTypeBuilder<User> builder)
//    {
//        builder.ToTable("Users");

//        builder.HasKey(x => x.Id);
//    }
//}

