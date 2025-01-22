
using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using YGZ.Identity.Domain.Authorizations;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Infrastructure.Persistence;
using YGZ.Identity.Infrastructure.Settings;
using YGZ.IdentityServer.Infrastructure.Settings;

namespace YGZ.Identity.Infrastructure.Extensions;

public static class IdentityExtension
{
    public static IServiceCollection AddIdentityExtension(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStrings.IdentityDb);
        var identityServerSettings = configuration.GetSection(IdentityServerSettings.SettingKey!).Get<IdentityServerSettings>()!;
        var jwtSettings = configuration.GetSection(JwtSettings.SettingKey!).Get<JwtSettings>()!;

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireLowercase = true;
        });

        // Configure token for email confirmation, password reset, change email, Two-Factor Authentication (2FA),... 
        services.Configure<DataProtectionTokenProviderOptions>(options =>
        {
            options.TokenLifespan = TimeSpan.FromDays(1);
        });

        // Add JWT Bearer Authentication
        services.AddJwtBearerAuthentication(configuration);

        // Add Policy Authorization
        services.AddPolicyAuthorization();

        // Add external login (Google, Facebook,...)
        services.AddExternalLogin(configuration);

        return services;
    }

    public static IServiceCollection AddPolicyAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(
                Policy.ReadAccess,
                policy =>
                {
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Read);
                }
            );
            options.AddPolicy(
                Policy.WriteAccess,
                policy =>
                {
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Write);
                }
            );
            options.AddPolicy(
                Policy.UpdateAccess,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Update);
                }
            );
            options.AddPolicy(
                Policy.DeleteAccess,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Delete);
                }
            );
        });

        return services;
    }

    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SettingKey!).Get<JwtSettings>()!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                //options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.Audience = jwtSettings.Audience;
                options.MetadataAddress = jwtSettings.MetadataAddress;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //ValidateIssuer = true,
                    //ValidateAudience = false,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    //ValidAudience = jwtSettings.ValidAudience,
                    //ClockSkew = TimeSpan.Zero,
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

        return services;
    }

    public static IServiceCollection AddExternalLogin(this IServiceCollection services, IConfiguration configuration)
    {
        var identityServerSettings = configuration.GetSection(IdentityServerSettings.SettingKey!).Get<IdentityServerSettings>()!;

        services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = identityServerSettings.ExternalOauthSettings.GoogleOauthSettings.ClientId;
                options.ClientSecret = identityServerSettings.ExternalOauthSettings.GoogleOauthSettings.ClientSecret;
                options.CallbackPath = identityServerSettings.ExternalOauthSettings.GoogleOauthSettings.CallbackPath;
                options.SignInScheme = IdentityConstants.ExternalScheme;
            });

        return services;
    }
}
