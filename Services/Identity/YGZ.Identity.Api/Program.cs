using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Serilog;
using YGZ.Identity.Api;
using YGZ.Identity.Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddPresentation().AddApplication();
builder.Host.AddSerilog(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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

app.UseSerilogRequestLogging();

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
