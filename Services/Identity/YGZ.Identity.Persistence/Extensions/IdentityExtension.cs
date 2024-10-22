using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Net;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using YGZ.Identity.Domain.Identity.Constants.Authorization;
using YGZ.Identity.Persistence.Infrastructure;
using YGZ.Identity.Domain.Identity.Entities;
using YGZ.Identity.Persistence.Data;
using Duende.IdentityServer;
using YGZ.Identity.Domain.IdentityServer.Authorization;

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
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddApiEndpoints();

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
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Write);
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

            options.AddPolicy(
                Policy.UpdateAccess,
                policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(JwtClaimTypes.Scope, ApiScope.Update);
                }
            );

            var defaultAuthorizationPolicy = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme
             //IdentityServerConstants.DefaultCookieAuthenticationScheme // for cookie authentication
             //IdentityServerConstants.ExternalCookieAuthenticationScheme // for external authentication (like Google, Facebook)
             );

            options.DefaultPolicy = defaultAuthorizationPolicy.RequireAuthenticatedUser().Build();
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer("Bearer", options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]!))
            };

             options.Events = new JwtBearerEvents
            {

                 OnAuthenticationFailed = context =>
                 {
                     context.Response.StatusCode = 401;
                     context.Response.ContentType = "application/json";
                     var apiPath = context.Request.Path.Value;
                     var method = context.Request.Method;
                     var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                     var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                     var timestamp = vietnamTime.ToString("yyyy-MM-ddTHH:mm:ss");  // Vietnam time in ISO 8601 format without 'Z'
                     var result = System.Text.Json.JsonSerializer.Serialize(new
                     {
                         title = "UnauthorizedException",
                         statusCode = HttpStatusCode.Unauthorized,
                         path = apiPath,
                         method,
                         message = "Authentication failed. Please check your token.",
                         timestamp,
                         error = context.Exception.Message
                     });
                     return context.Response.WriteAsync(result);
                 },
                 OnChallenge = context =>
                 {
                     var apiPath = context.Request.Path.Value;
                     var method = context.Request.Method;
                     var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                     var vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                     var timestamp = vietnamTime.ToString("yyyy-MM-ddTHH:mm:ss");  // Vietnam time in ISO 8601 format without 'Z'
                     // Ensure that the response hasn't started before modifying it
                     if (!context.Response.HasStarted)
                     {
                         context.HandleResponse();
                         context.Response.StatusCode = 401;
                         context.Response.ContentType = "application/json";
                         var result = System.Text.Json.JsonSerializer.Serialize(new
                         {
                             title = "UnauthorizedException",
                             statusCode = HttpStatusCode.Unauthorized,
                             path = apiPath,
                             method,
                             message = "You are not authorized to access this resource.",
                             timestamp,
                             error = "Invalid or missing token."
                         });
                         return context.Response.WriteAsync(result);
                     }
                     else
                     {
                         // Log that the response has already started
                         Console.WriteLine("Response already started. Cannot modify the response.");
                         return Task.CompletedTask;
                     }
                 },
             };
            });

        return services;
    }
}
