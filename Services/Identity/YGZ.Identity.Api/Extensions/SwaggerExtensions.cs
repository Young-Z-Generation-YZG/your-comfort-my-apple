using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using NSwag;
using YGZ.Identity.Api.Contracts.Addresses;
using YGZ.Identity.Api.Contracts.Profiles;
using YGZ.Identity.Api.Contracts.Auth;

namespace YGZ.Identity.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddOpenApiDocument((settings, sp) =>
        {
            var keycloakOptions = sp.GetRequiredService<IOptionsMonitor<KeycloakAuthenticationOptions>>()?.Get(JwtBearerDefaults.AuthenticationScheme)!;

            settings.Title = "Identity APIs";

            settings.AddSecurity(
                JwtBearerDefaults.AuthenticationScheme,
                [],
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                }
            );

            settings.AddSecurity(
                OpenIdConnectDefaults.AuthenticationScheme,
                [],
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OpenIdConnect,
                    OpenIdConnectUrl = "http://localhost:17070/realms/ygz-realm/.well-known/openid-configuration",
                }
            );


            settings.OperationProcessors.Add(new OperationSecurityScopeProcessor(OpenIdConnectDefaults.AuthenticationScheme));
            settings.OperationProcessors.Add(new OperationSecurityScopeProcessor(JwtBearerDefaults.AuthenticationScheme));

            settings.OperationProcessors.Add(new AccessOtpRequestExample());

            // Add the custom schema processor for LoginRequest examples
            settings.SchemaSettings.SchemaProcessors.Add(new LoginRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new RegisterRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new AddAddressRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new UpdateAddressRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new UpdateProfileRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new ChangePasswordRequestExample());
        });

        return services;
    }

    public static void SwaggerOAuthSettings(this SwaggerUiSettings ui, IConfiguration configuration)
    {
        var keycloakOptions = configuration.GetKeycloakOptions<KeycloakAuthenticationOptions>()!;

        ui.OAuth2Client = new OAuth2ClientSettings
        {
            ClientId = keycloakOptions.Resource,
            ClientSecret = keycloakOptions?.Credentials?.Secret,
        };
    }
}
