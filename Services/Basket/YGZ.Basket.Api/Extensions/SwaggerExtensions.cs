using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using NSwag;
using YGZ.Basket.Api.Contracts;

namespace YGZ.Basket.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerExtensions(this IServiceCollection services)
    {
        services.AddOpenApiDocument((settings, serviceProvider) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptionsMonitor<KeycloakAuthenticationOptions>>()?.Get(JwtBearerDefaults.AuthenticationScheme)!;

            settings.Title = "Basket API";

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

            settings.SchemaSettings.SchemaProcessors.Add(new StoreBasketRequestExample());

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
