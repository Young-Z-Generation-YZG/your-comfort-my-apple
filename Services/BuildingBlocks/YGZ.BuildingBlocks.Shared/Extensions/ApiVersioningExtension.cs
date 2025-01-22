

using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace YGZ.BuildingBlocks.Shared.Extensions;

public static class ApiVersioningExtension
{
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
            // Specify the format of the version number
            options.GroupNameFormat = "'v'VVV";

            // Specify the version number in the URL
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}
