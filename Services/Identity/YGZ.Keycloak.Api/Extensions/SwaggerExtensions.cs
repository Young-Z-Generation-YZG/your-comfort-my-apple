using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using YGZ.Keycloak.Api.Contracts;

namespace YGZ.Keycloak.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerExtensions(this IServiceCollection services)
    {
        services.AddOpenApiDocument((document, sp) =>
        {
            var keycloakOptions = sp.GetRequiredService<IOptionsMonitor<KeycloakAuthenticationOptions>>()?.Get(JwtBearerDefaults.AuthenticationScheme)!;

            document.Title = "Keycloak Identity API";

            document.AddSecurity(
                JwtBearerDefaults.AuthenticationScheme,
                [],
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                }
            );

            document.AddSecurity(
                OpenIdConnectDefaults.AuthenticationScheme,
                [],
                new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.OpenIdConnect,
                    OpenIdConnectUrl = "http://localhost:17070/realms/ygz-realm/.well-known/openid-configuration",
                }
            );


            document.OperationProcessors.Add(new OperationSecurityScopeProcessor(OpenIdConnectDefaults.AuthenticationScheme));
            document.OperationProcessors.Add(new OperationSecurityScopeProcessor(JwtBearerDefaults.AuthenticationScheme));

            // Add the custom schema processor for LoginRequest examples
            document.SchemaSettings.SchemaProcessors.Add(new LoginRequestExample());
            document.SchemaSettings.SchemaProcessors.Add(new RegisterRequestExample());
        });

        return services;
    }

    public static void UseApplicationSwaggerSettings(this SwaggerUiSettings ui, IConfiguration configuration)
    {
        var keycloakOptions = configuration.GetKeycloakOptions<KeycloakAuthenticationOptions>()!;

        ui.OAuth2Client = new OAuth2ClientSettings
        {
            ClientId = keycloakOptions.Resource,
            ClientSecret = keycloakOptions?.Credentials?.Secret,
        };
    }
}
