

using HealthChecks.UI.Client;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api;
using YGZ.Catalog.Api.Extensions;
using YGZ.Catalog.Api.GrpcServices;
using YGZ.Catalog.Application;
using YGZ.Catalog.Infrastructure;
using YGZ.Catalog.Infrastructure.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var host = builder.Host;

// Add Layers
services
    .AddPresentationLayer()
    .AddInfrastructureLayer(builder.Configuration)
    .AddApplicationLayer(builder.Configuration);

// Add services to the container.
services.AddProblemDetails();
services.AddSwaggerExtension();

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});

services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());

services.AddControllers(options => options.AddProtectedResources())
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true; // Optional
        });

// Add Serilog
host.AddSerilogExtension(builder.Configuration);

services
    .AddHealthChecks()
    .AddMongoDb(builder.Configuration.GetConnectionString("CatalogDb")!);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();


app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(ui => ui.SwaggerOAuthSettings(builder.Configuration));

    // Seed data initialization - wrap in try-catch to prevent startup failure
    try
    {
        await app.ApplySeedDataAsync();
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Failed to apply seed data. Application will continue without seed data.");
    }
}

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    using var scope = app.Services.CreateScope();
    var redisConnection = scope.ServiceProvider.GetRequiredService<StackExchange.Redis.IConnectionMultiplexer>();
    var server = redisConnection.GetServer(redisConnection.GetEndPoints().First());
    server.FlushDatabase();
});

app.UseStatusCodePages();

//app.UseHttpsRedirection();

app.UseStatusCodePages();
app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<CatalogRpcService>();
app.MapGrpcReflectionService();

app.Run();
