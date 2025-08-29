

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace YGZ.BuildingBlocks.Shared.Extensions;

public static class SerilogExtension
{
    public static void AddSerilogExtension(this IHostBuilder host, IConfiguration configuration)
    {
        // override the default logging configuration with Serilog
        host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration);
        });
    }
}
