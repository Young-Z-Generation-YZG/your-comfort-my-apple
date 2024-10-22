using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using YGZ.Identity.Api;
using YGZ.Identity.Api.Common.Extensions;
using YGZ.Identity.Application;
using YGZ.Identity.Infrastructure;
using YGZ.Identity.Persistence;
using YGZ.Identity.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddApplicationLayer()
    .AddPersistenceLayer(builder.Configuration)
    .AddInfrastructureLayer(builder.Configuration)
    .AddPresentationLayer();

builder.Host.AddSerilog(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await IdentityServerMigrationExtension.ApplyMigrationsAsync(app);

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Get the API version descriptions
        IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();
        // Add the Swagger endpoints for each API version
        foreach (var description in descriptions)
        {
            // Create the Swagger endpoint URL
            string url = $"/swagger/{description.GroupName}/swagger.json";
            // Create the Swagger endpoint name
            string name = description.GroupName.ToUpperInvariant();
            // Add the Swagger endpoint to the Swagger UI
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});

app.UseSerilogRequestLogging();


app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.MapControllers();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();


app.Run();
