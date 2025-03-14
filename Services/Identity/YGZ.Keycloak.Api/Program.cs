using Keycloak.AuthServices.Authorization;
using YGZ.Keycloak.Api;
using YGZ.Keycloak.Api.Extensions;
using YGZ.Keycloak.Application;
using YGZ.Keycloak.Infrastructure;
using YGZ.Keycloak.Infrastructure.Persistence.Extensions;
using YGZ.BuildingBlocks.Shared.Extensions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
services.AddSwaggerExtensions();

services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());


services.AddControllers(options => options.AddProtectedResources())
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true; // Optional
        });

services.AddEndpointsApiExplorer();

// Add Serilog
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

var app = builder.Build();

app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(ui => ui.UseApplicationSwaggerSettings(builder.Configuration));

    app.ApplyMigrations();
    await app.ApplySeedDataAsync();
}

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});

app.UseHttpsRedirection();
app.UseExceptionHandler("/error");

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.Run();
