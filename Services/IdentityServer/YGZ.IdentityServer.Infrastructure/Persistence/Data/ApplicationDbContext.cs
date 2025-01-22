


//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using YGZ.IdentityServer.Domain.Users;

//namespace YGZ.IdentityServer.Infrastructure.Persistence.Data;

//public class ApplicationDbContext : IdentityDbContext<User>
//{
//    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


//    protected override void OnModelCreating(ModelBuilder builder)
//    {
//        base.OnModelCreating(builder);

//        foreach (var entityType in builder.Model.GetEntityTypes())
//        {
//            var tableName = entityType.GetTableName()!;

//            if (tableName.StartsWith("AspNet"))
//            {
//                entityType.SetTableName(tableName.Substring(6));
//            }
//        }
//    }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//    {
//        base.OnConfiguring(optionsBuilder);
        
        
//    }
//}
