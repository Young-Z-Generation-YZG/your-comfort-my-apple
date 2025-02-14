

using System.Security.Claims;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Keycloak.AuthServices.Common;
using Keycloak.AuthServices.Sdk;
using Keycloak.AuthServices.Sdk.Kiota;
using KeycloakAdminClientOptions = Keycloak.AuthServices.Sdk.Kiota.KeycloakAdminClientOptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using YGZ.Identity.Domain.Authorizations;
using YGZ.Identity.Infrastructure.Settings;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization;

namespace YGZ.Identity.Infrastructure.Extensions;

public static class KeyCloakIdentityServerExtension
{
    public static IServiceCollection AddKeycloakIdentityServerExtension(this IServiceCollection services, IConfiguration configuration)
    {
        //var jwtSettings = configuration.GetSection(JwtSettings.SettingKey!).Get<JwtSettings>()!;

        //services.AddControllers(options => options.AddProtectedResources());

        //services
        //    .AddKeycloakWebApiAuthentication(configuration,
        //        options =>
        //        {
        //            options.RequireHttpsMetadata = false;
        //            //options.Audience = jwtSettings.Audience;
        //            //options.MetadataAddress = jwtSettings.MetadataAddress;
        //            //options.TokenValidationParameters = new TokenValidationParameters
        //            //{
        //            //    ValidIssuer = jwtSettings.ValidIssuer,
        //            //};
        //        }
        //);


        //services
        //    .AddAuthorization(options =>
        //    {
        //        options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

        //        options.AddPolicy(
        //            AuthorizationConstants.Policies.RequireAspNetCoreRole,
        //            policy => policy.RequireRole(AuthorizationConstants.Roles.AspNetCoreRole)
        //        );

        //        options.AddPolicy(
        //            AuthorizationConstants.Policies.RequireRealmRole,
        //            policy => policy.RequireRealmRoles(AuthorizationConstants.Roles.RealmRole)
        //        );

        //        options.AddPolicy(
        //            AuthorizationConstants.Policies.RequireClientRole,
        //            policy => policy.RequireResourceRoles(AuthorizationConstants.Roles.ClientRole)
        //        );

        //        options.AddPolicy(
        //                AuthorizationConstants.Policies.RequireToBeInKeycloakGroupAsReader,
        //                policy => policy.RequireProtectedResource("workspaces", "workspaces:read")
        //        );
        //    })
        //    .AddKeycloakAuthorization()
        //    .AddAuthorizationServer(configuration);

        //services.AddSingleton<IAuthorizationPolicyProvider, ProtectedResourcePolicyProvider>();


        //services
        //    .AddKeycloakAuthorization(configuration)
        //    .AddAuthorizationServer(configuration);

        //services
        //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddKeycloakWebApi(configuration);


        //services.AddSingleton<IClaimsTransformation, KeycloakClaimsTransformation>();

        return services;
    }
}
