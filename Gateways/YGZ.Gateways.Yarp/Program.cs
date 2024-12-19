using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

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

// Configure the HTTP request pipeline.
app.MapReverseProxy();

app.Run();
