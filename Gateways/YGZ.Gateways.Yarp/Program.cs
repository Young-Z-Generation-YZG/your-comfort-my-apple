using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using YGZ.Gateways.Yarp.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Logging.AddOpenTelemetry(logging =>
{
    logging.IncludeFormattedMessage = true;
    logging.IncludeScopes = true;
});

builder.Services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());

services.AddSwaggerExtensions();

services
    .AddOpenTelemetry()
    .WithMetrics(metrics =>
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddKeycloakAuthServicesInstrumentation()
    )
    .WithTracing(tracing =>
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddKeycloakAuthServicesInstrumentation()
    )
    .UseOtlpExporter();

services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddKeycloakWebApi(builder.Configuration);

services
    .AddAuthorization()
    .AddKeycloakAuthorization()
    .AddAuthorizationServer(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();

    app.UseOpenApi();
    app.UseSwaggerUi(ui => ui.UseApplicationSwaggerSettings(builder.Configuration));
}

// HTTPS is handled by nginx, not the Gateway
// app.UseHttpsRedirection();

// Enable CORS
app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});


app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();
app.MapControllers();

app.Run();
