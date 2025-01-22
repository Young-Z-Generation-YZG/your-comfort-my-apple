using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();

var test = builder.Configuration["JwtSettings:ValidIssuer"];

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.Audience = builder.Configuration["JwtSettings:Audience"];
        options.MetadataAddress = builder.Configuration["JwtSettings:MetadataAddress"]!;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtSettings:ValidIssuer"],
        };

    });

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService("YGZ.Gateways.Yarp"))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation();

        tracing.AddOtlpExporter();
    });

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opts =>
    {
        opts.Window = TimeSpan.FromSeconds(10);
        opts.PermitLimit = 10;
    });
});

var app = builder.Build();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapReverseProxy();

app.Run();
