using System.Security.Claims;
using System.Text;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using YGZ.BuildingBlocks.Shared.Extensions;
using YGZ.Identity.Api;
using YGZ.Identity.Api.Common.Extensions;
using YGZ.Identity.Application;
using YGZ.Identity.Domain.Users;
using YGZ.Identity.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services
    .AddApplicationLayer(builder.Configuration)
    .AddInfrastructureLayer(builder.Configuration)
    .AddPresentationLayer(builder.Configuration);
       

//builder.Services.AddIdentityApiEndpoints<User>();

// Razor Pages
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Serilog
builder.Host.AddSerilogExtension(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

        foreach (ApiVersionDescription description in descriptions)
        {
            string url = $"/swagger/{description.GroupName}/swagger.json";
            string name = description.GroupName.ToUpperInvariant();

            options.SwaggerEndpoint(url, name);
        }
    });

    app.ApplyMigrations();
}



//app.MapIdentityApi<User>();

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});

app.UseStaticFiles();

// app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.Run();
