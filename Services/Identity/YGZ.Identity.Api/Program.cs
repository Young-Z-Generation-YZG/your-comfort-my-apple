
using HealthChecks.UI.Client;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using YGZ.Identity.Api;
using YGZ.Identity.Api.Extensions;
using YGZ.Identity.Application;
using YGZ.Identity.Infrastructure;
using YGZ.Identity.Infrastructure.Persistence.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting YGZ.Identity.Api");

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add Layers
services
    .AddPresentationLayer(builder)
    .AddInfrastructureLayer(builder.Configuration)
    .AddApplicationLayer(builder.Configuration);

services.AddProblemDetails();
services.AddHttpContextAccessor();
// services.AddScoped<TenantProvider>();
services.AddEndpointsApiExplorer();
services.AddControllers(options => options.AddProtectedResources());

services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(ui => ui.SwaggerOAuthSettings(builder.Configuration));

    app.ApplyMigrations();
    await app.ApplySeedDataAsync();
}

// Middleware configurations
app.UseSerilogRequestLogging();

// Health checks
app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Enable CORS
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
