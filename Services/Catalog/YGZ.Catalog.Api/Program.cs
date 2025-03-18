

using HealthChecks.UI.Client;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Catalog.Api;
using YGZ.Catalog.Api.Extensions;
using YGZ.Catalog.Application;
using YGZ.Catalog.Infrastructure;

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
    app.UseSwaggerUi(ui => ui.UseApplicationSwaggerSettings(builder.Configuration));

    //using var scope = app.Services.CreateScope();
    //await app.ApplySeedDataAsync();
}

app.UseStatusCodePages();

//app.UseHttpsRedirection();

app.UseStatusCodePages();
app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
