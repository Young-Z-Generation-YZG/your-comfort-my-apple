
using HealthChecks.UI.Client;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using YGZ.Identity.Api;
using YGZ.Identity.Api.Extensions;
using YGZ.Identity.Application;
using YGZ.Identity.Infrastructure;
using YGZ.Identity.Infrastructure.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add Layers
services
    .AddPresentationLayer(builder)
    .AddInfrastructureLayer(builder.Configuration)
    .AddApplicationLayer(builder.Configuration);


services.AddProblemDetails();
services.AddHttpContextAccessor();
services.AddEndpointsApiExplorer();
services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());
services.AddControllers(options => options.AddProtectedResources())
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true; // Optional
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(ui => ui.SwaggerOAuthSettings(builder.Configuration));

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
