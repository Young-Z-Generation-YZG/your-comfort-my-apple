
using System.Security.Claims;
using System.Text;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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

        //services.Configure<IdentityOptions>(options =>
        //{
        //    options.Password.RequireLowercase = true;
        //});

        // Configure token for email confirmation, password reset, change email, Two-Factor Authentication (2FA),... 
        //services.Configure<DataProtectionTokenProviderOptions>(options =>
        //{
        //    options.TokenLifespan = TimeSpan.FromDays(1);
        //});

        // Add JWT Bearer Authentication
        //services.AddJwtBearerAuthentication(configuration);

        // Add Policy Authorization
        //services.AddPolicyAuthorization();

        // Add external login (Google, Facebook,...)
        //services.AddExternalLogin(configuration);

        return services;
    }

    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection(JwtSettings.SettingKey!).Get<JwtSettings>()!;

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = configuration["JwtSettings:Issuers"] ?? "default-issuers",
                        ValidAudience = configuration["JwtSettings:Audience"] ?? "default-audience",
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"] ?? "default-secret-key"))
                    };
                });

        services.AddAuthorization();

        //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddJwtBearer(options =>
        //    {
        //        //options.SaveToken = true;
        //        options.RequireHttpsMetadata = false;
        //        options.Audience = jwtSettings.Audience;
        //        options.MetadataAddress = jwtSettings.MetadataAddress;
        //        options.TokenValidationParameters = new TokenValidationParameters
        //        {
        //            //ValidateIssuer = true,
        //            //ValidateAudience = false,
        //            ValidIssuer = jwtSettings.ValidIssuer,
        //            //ValidAudience = jwtSettings.ValidAudience,
        //            //ClockSkew = TimeSpan.Zero,
        //            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        //        };

        //        //options.Events = new JwtBearerEvents
        //        //{
        //        //    OnTokenValidated = context =>
        //        //    {
        //        //        var user = context.Principal;
        //        //        var identity = (ClaimsIdentity)user!.Identity!;

        //        //        // Log all claims for debugging
        //        //        Console.WriteLine("Claims from Keycloak:");
        //        //        foreach (var claim in user.Claims)
        //        //        {
        //        //            Console.WriteLine($"{claim.Type}: {claim.Value}");
        //        //        }

        //        //        // Extract roles from resource_access.admin-rest-api.roles
        //        //        var resourceAccessClaim = user.FindFirst("resource_access");
        //        //        if (resourceAccessClaim != null)
        //        //        {
        //        //            var resourceAccessJson = JObject.Parse(resourceAccessClaim.Value);
        //        //            var roles = resourceAccessJson["nextjs-confidential"]?["roles"]?.ToObject<List<string>>();

        //        //            if (roles != null)
        //        //            {
        //        //                foreach (var role in roles)
        //        //                {
        //        //                    identity.AddClaim(new Claim(ClaimTypes.Role, role)); // Map role properly
        //        //                    Console.WriteLine($"Added Role: {role}");
        //        //                }
        //        //            }
        //        //        }

        //        //        return Task.CompletedTask;
        //        //    }
        //        //};

        //    });

        return services;
    }

    public static IServiceCollection AddPolicyAuthorization(this IServiceCollection services)
    {
        // define policies
        // services.AddAuthorization(options =>
        // {
        //     // define api scopes (does the token allow API access?)
        //     //options.AddPolicy(
        //     //    Policies.ApiScope.ReadAccess,
        //     //    policy =>
        //     //    {
        //     //        policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Read);
        //     //    }
        //     //);
        //     //options.AddPolicy(
        //     //    Policies.ApiScope.WriteAccess,
        //     //    policy =>
        //     //    {
        //     //        policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Write);
        //     //    }
        //     //);
        //     //options.AddPolicy(
        //     //    Policies.ApiScope.UpdateAccess,
        //     //    policy =>
        //     //    {
        //     //        policy.RequireAuthenticatedUser();
        //     //        policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Update);
        //     //    }
        //     //);
        //     //options.AddPolicy(
        //     //    Policies.ApiScope.DeleteAccess,
        //     //    policy =>
        //     //    {
        //     //        policy.RequireAuthenticatedUser();
        //     //        policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Delete);
        //     //    }
        //     //);

       
        //     // define conditional policies
        // });

        return services;
    }


    public static IServiceCollection AddExternalLogin(this IServiceCollection services, IConfiguration configuration)
    {
        //var identityServerSettings = configuration.GetSection(IdentityServerSettings.SettingKey!).Get<IdentityServerSettings>()!;

        //services.AddAuthentication()
        //    .AddGoogle(options =>
        //    {
        //        options.ClientId = identityServerSettings.ExternalOauthSettings.GoogleOauthSettings.ClientId;
        //        options.ClientSecret = identityServerSettings.ExternalOauthSettings.GoogleOauthSettings.ClientSecret;
        //        options.CallbackPath = identityServerSettings.ExternalOauthSettings.GoogleOauthSettings.CallbackPath;
        //        options.SignInScheme = IdentityConstants.ExternalScheme;
        //    });

        return services;
    }
}
