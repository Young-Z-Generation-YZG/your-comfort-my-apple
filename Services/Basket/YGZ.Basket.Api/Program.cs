using HealthChecks.UI.Client;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using YGZ.Basket.Api;
using YGZ.Basket.Api.Extensions;
using YGZ.Basket.Application;
using YGZ.Basket.Infrastructure;
using YGZ.Basket.Infrastructure.Persistence.Extensions;
using YGZ.BuildingBlocks.Shared.Extensions;

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

services.AddEndpointsApiExplorer();

// Add Serilog
host.AddSerilogExtension(builder.Configuration);

services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("BasketDb")!)
    .AddRedis(builder.Configuration.GetConnectionString("RedisDb")!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(ui => ui.SwaggerOAuthSettings(builder.Configuration));

    await app.ApplySeedDataAsync();
}

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Register shutdown hook to clear cache
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    using var scope = app.Services.CreateScope();
    var redisConnection = scope.ServiceProvider.GetRequiredService<StackExchange.Redis.IConnectionMultiplexer>();
    var server = redisConnection.GetServer(redisConnection.GetEndPoints().First());
    server.FlushDatabase();
});

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});


app.UseStatusCodePages();

//app.UseHttpsRedirection();
app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
