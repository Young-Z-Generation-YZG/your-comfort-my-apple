

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace YGZ.BuildingBlocks.Shared.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services, Assembly assembly, string authorizationUrl)
    {
        services.AddSwaggerGen(options =>
        {
            //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            //{
            //    In = ParameterLocation.Header,
            //    Description = "Please insert JWT with Bearer into field",
            //    Name = "Authorization",
            //    Type = SecuritySchemeType.Http,
            //    BearerFormat = "JWT",
            //    Scheme = "Bearer"
            //});
            options.CustomSchemaIds(id => id.FullName!.Replace("+", "-"));

            options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(authorizationUrl),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "openid" },
                            { "profile", "profile" }
                        }
                    }
                }
            });

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Keycloak",
                            Type = ReferenceType.SecurityScheme
                        },
                        In = ParameterLocation.Header,
                        Name = "Bearer",
                        Scheme = "Bearer"
                    },
                    new string[] { }
                }
            };

            options.AddSecurityRequirement(securityRequirement);

            // add swagger examples
            options.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblies(assembly);

        return services;
    }
}
