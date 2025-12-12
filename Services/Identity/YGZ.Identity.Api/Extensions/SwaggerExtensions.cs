using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.AspNetCore;
using NSwag.Generation.Processors.Security;
using YGZ.Identity.Api.Contracts.Addresses;
using YGZ.Identity.Api.Contracts.Auth;
using YGZ.Identity.Api.Contracts.Profiles;
using YGZ.Identity.Api.Contracts.Tenants;
using YGZ.Identity.Api.Contracts.Users;

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

            // Add custom headers to all operations
            // settings.OperationProcessors.Add(new CustomHeaderOperationProcessor());

            // Add processor for SwaggerHeader attributes
            settings.OperationProcessors.Add(new SwaggerHeaderOperationProcessor());

            settings.OperationProcessors.Add(new AccessOtpRequestExample());
            settings.OperationProcessors.Add(new GetListUsersRequestExample());

            // Add the custom schema processor for LoginRequest examples
            settings.SchemaSettings.SchemaProcessors.Add(new LoginRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new RegisterRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new AddAddressRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new UpdateAddressRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new UpdateProfileRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new ChangePasswordRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new CreateTenantUserRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new AssignRolesRequestExample());
            settings.SchemaSettings.SchemaProcessors.Add(new AddNewStaffRequestExample());
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

        // Configure additional Swagger UI settings for better header visibility
        ui.DefaultModelsExpandDepth = 1;
        ui.DefaultModelExpandDepth = 1;
    }
}
