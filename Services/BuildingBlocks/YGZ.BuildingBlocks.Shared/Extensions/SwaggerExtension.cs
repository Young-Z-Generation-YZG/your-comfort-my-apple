

using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace YGZ.BuildingBlocks.Shared.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services, Assembly assembly, string authorizationUrl)
    {
        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "JWT Authentication",
            Description = "Enter JWT Bearer token **_only_**",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme, // must be lower case
            BearerFormat = "JWT",
        };

        var securityRequirement = new OpenApiSecurityRequirement
        {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    },
                    Array.Empty<string>()
                }
        };

        services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(id => id.FullName!.Replace("+", "-"));

            // Define security Schema for swagger with Bearer JWT token
            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

            // Apply bearer token in header for all requests
            options.AddSecurityRequirement(securityRequirement);


            //options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
            //{
            //    Type = SecuritySchemeType.OAuth2,
            //    Flows = new OpenApiOAuthFlows
            //    {
            //        Implicit = new OpenApiOAuthFlow
            //        {
            //            AuthorizationUrl = new Uri(authorizationUrl),
            //            Scopes = new Dictionary<string, string>
            //            {
            //                { "openid", "openid" },
            //                { "profile", "profile" }
            //            }
            //        },
            //        Password = new OpenApiOAuthFlow
            //        {
            //            TokenUrl = new Uri("http://localhost:17070/realms/ygz-ecommerce-auth/protocol/openid-connect/token"),
            //            Scopes = new Dictionary<string, string>
            //            {
            //                { "openid", "openid" },
            //                { "profile", "profile" },
            //            }
            //        }
            //    }
            //});

            //var securityRequirement = new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Id = "Keycloak",
            //                Type = ReferenceType.SecurityScheme
            //            },
            //            In = ParameterLocation.Header,
            //            Name = "Bearer",
            //            Scheme = "Bearer"
            //        },
            //        new string[] { }
            //    }
            //};

            //options.AddSecurityRequirement(new OpenApiSecurityRequirement
            //{
            //    {
            //        new OpenApiSecurityScheme
            //        {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "Bearer"
            //            }
            //        },
            //        Array.Empty<string>()
            //    }
            //});



            // add swagger examples
            options.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblies(assembly);

        return services;
    }
}
