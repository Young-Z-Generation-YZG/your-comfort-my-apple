using Asp.Versioning.ApiExplorer;
using Serilog;
using YGZ.Catalog.Api;
using YGZ.Catalog.Application;
using YGZ.Catalog.Infrastructure;
using YGZ.Catalog.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddPresentationLayer()
    .AddApplicationLayer(builder.Configuration)
    .AddPersistenceLayer(builder.Configuration)
    .AddInfrastructureLayer(builder.Configuration);

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
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors(options =>
{
    options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
});

app.UseExceptionHandler("/error");

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseRequestContextLogging();

app.Run();
