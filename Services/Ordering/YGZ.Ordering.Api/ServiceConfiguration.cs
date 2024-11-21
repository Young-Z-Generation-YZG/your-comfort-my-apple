using Asp.Versioning;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using YGZ.Ordering.Api.Common.Errors;
using YGZ.Ordering.Api.Common.Mappings;
using YGZ.Ordering.Api.OpenApi;

namespace YGZ.Ordering.Api;

public static class ServiceConfiguration
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddSwaggerExtension();

        services.AddMappings();

        services.AddApiVersioningExtension();

        services.AddGlobalExceptionHandler();

        return services;
    }

    public static void AddSerilogExtension(this IHostBuilder host, IConfiguration configuration)
    {
        host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration);
        });
    }

    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

            options.ExampleFilters();
        });

        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = ApiVersion.Default;

            options.ApiVersionReader = new UrlSegmentApiVersionReader();
            options.ReportApiVersions = true;

        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfiguration>();

        return services;
    }

    public static IServiceCollection AddApiVersioningExtension(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            // If the client does not specify the version, use the default version number
            options.DefaultApiVersion = new ApiVersion(1, 0);

            // Specify the default version for the API if the client does not specify the version (above)
            options.AssumeDefaultVersionWhenUnspecified = true;

            // Specify the version number of the API that the client must use
            options.ReportApiVersions = true;

            // Read the version number from the URL segment
            options.ApiVersionReader = new UrlSegmentApiVersionReader();

        }).AddApiExplorer(options =>
        {
            // this is the group name format, it will format the group name (v1.0) to ('v'VVV) => 'v1.0
            options.GroupNameFormat = "'v'VVV";

            // this is the group name selector, it will select the group name based on the version (only necsesary with url segment)
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, OrderProblemDetailsFactory>();

        return services;
    }
}