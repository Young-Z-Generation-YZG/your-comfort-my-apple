using YGZ.Identity.Api.OpenApi;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Asp.Versioning;
using Serilog;
using Mapster;
using System.Reflection;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using YGZ.Identity.Api.Common.Errors;
using Microsoft.OpenApi.Models;

namespace YGZ.Identity.Api;
public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services) 
    {
        services.AddVersioning();

        services.AddSwaggerExtension();

        services.AddMapping();

        services.AddRazorPages();

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
                Scheme = "bearer"
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
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfiguration>();

        return services;
    }

    public static IServiceCollection AddVersioning(this IServiceCollection services)
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

    public static void AddSerilog(this IHostBuilder builder, IConfiguration configuration)
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
        services.AddSingleton<ProblemDetailsFactory, IdentityProblemDetailsFactory>();

        return services;
    }
}
