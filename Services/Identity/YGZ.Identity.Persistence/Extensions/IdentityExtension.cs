using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Identity.Domain.Identity.Entities;
using YGZ.Identity.Persistence.Data;  // Updated namespace
using Microsoft.IdentityModel.Tokens;
using System.Text;
using YGZ.Identity.Persistence.Infrastructure;

namespace YGZ.Identity.Persistence.Extensions;
public static class IdentityExtension
{
    public static IServiceCollection AddIdentityExtension(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionString.SettingKey);

        // Add ApplicationDbContext
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(
            connectionString,
            options => options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
        ));

        // Add Identity
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;

            //options.SignIn.RequireConfirmedEmail = true;

            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()  // Use ApplicationDbContext here
        .AddApiEndpoints();


        services.AddAuthentication("Bearer")
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
            };
            });

        return services;
    }
}
