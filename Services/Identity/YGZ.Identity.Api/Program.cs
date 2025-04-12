
using HealthChecks.UI.Client;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Api;
using YGZ.Identity.Api.Extensions;
using YGZ.Identity.Application;
using YGZ.Identity.Infrastructure;
using YGZ.Identity.Infrastructure.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var host = builder.Host;

// Add Layers
services
    .AddPresentationLayer()
    .AddInfrastructureLayer(builder.Configuration)
    .AddApplicationLayer(builder.Configuration);

services.AddProblemDetails();
services.AddSwaggerExtensions();

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});

builder.Services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());

// Add services to the container.
services.AddControllers(options => options.AddProtectedResources())
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true; // Optional
        });

host.AddSerilogExtension(builder.Configuration);

services.AddHealthChecks()
    .AddNpgSql(
        connectionString: builder.Configuration.GetConnectionString("IdentityDb")!,
        name: "IdentityDb",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "postgres" })
    .AddNpgSql(
        connectionString: builder.Configuration.GetConnectionString("KeycloakDb")!,
        name: "KeycloakDb",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "postgres" });

services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(ui => ui.UseApplicationSwaggerSettings(builder.Configuration));

     app.ApplyMigrations();
    await app.ApplySeedDataAsync();
}

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});


app.UseStatusCodePages();
app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
