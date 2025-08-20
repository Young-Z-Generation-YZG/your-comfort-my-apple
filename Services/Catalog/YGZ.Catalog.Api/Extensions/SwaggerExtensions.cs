using Keycloak.AuthServices.Common;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;

using Keycloak.AuthServices.Authentication;
using YGZ.Catalog.Api.Contracts.IPhone16Request;
using YGZ.Catalog.Api.Contracts.CategoryRequest;
using YGZ.Catalog.Api.Contracts;
using YGZ.Catalog.Api.Contracts.modelRequest;
using YGZ.Catalog.Api.Contracts.ReviewRequest;
using YGZ.Catalog.Api.Contracts.PromotionRequest;

namespace YGZ.Catalog.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddOpenApiDocument((settings, serviceProvider) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptionsMonitor<KeycloakAuthenticationOptions>>()?.Get(JwtBearerDefaults.AuthenticationScheme)!;

            settings.Title = "Catalog API";

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
            settings.OperationProcessors.Add(new GetProductsRequestExample());
            settings.OperationProcessors.Add(new GetModelsRequestExample());
            settings.OperationProcessors.Add(new GetReviewsByModelRequestExample());

            settings.SchemaSettings.SchemaProcessors.Add(new CreateCategoryRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new CreateIPhone16ModelRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new CreateIPhone16DetailRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new CreateReviewRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new UpdateReviewRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new CreatePromotionEventRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new CreatePromotionGlobalRequestExample());

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
