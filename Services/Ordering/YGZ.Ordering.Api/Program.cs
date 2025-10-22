using HealthChecks.UI.Client;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Ordering.Api;
using YGZ.Ordering.Api.Extensions;
using YGZ.Ordering.Application;
using YGZ.Ordering.Infrastructure;
using YGZ.Ordering.Infrastructure.Persistence.Extensions;

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

services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());


services.AddControllers(options => options.AddProtectedResources())
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true; // Optional
        });

builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("OrderingDb")!);

// Add Serilog
host.AddSerilogExtension(builder.Configuration);

var app = builder.Build();

app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(ui => ui.SwaggerOAuthSettings(builder.Configuration));

    await app.ApplyMigrationAsync();
    //await app.ApplySeedDataAsync();
}

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});

//app.UseHttpsRedirection();
app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcReflectionService();
app.MapControllers();
// Add this line to map gRPC endpoints
app.MapGrpcEndpoints();

app.Run();