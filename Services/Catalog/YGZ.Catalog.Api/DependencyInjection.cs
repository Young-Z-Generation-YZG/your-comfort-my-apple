using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using YGZ.Catalog.Api.OpenApi;
using Serilog;
using Mapster;
using MapsterMapper;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using YGZ.Catalog.Api.Common.Errors;
using Asp.Versioning;
using Swashbuckle.AspNetCore.Filters;

namespace YGZ.Catalog.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddSwaggerExtension();

        services.AddApiVersioningExtension();

        services.AddMapping();

        services.AddGlobalExceptionHandler();

        return services;
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
            //semantic versioning
            //first character is the principal or greater version
            //second character is the minor version
            //third character is the patch
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        //services.AddSwaggerExamplesFromAssemblies(Assembly.GetEntryAssembly());

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

    public static void AddSerilogExtension(this IHostBuilder builder, IConfiguration configuration)
    {
        builder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration);
        });
    }

    public static IServiceCollection AddMapping(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();

        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);

        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
    {
        services.AddSingleton<ProblemDetailsFactory, CatalogProblemDetailsFactory>();

        return services;
    }
}
