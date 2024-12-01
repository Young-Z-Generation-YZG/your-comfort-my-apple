using Asp.Versioning.ApiExplorer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using YGZ.Basket.Api;
using YGZ.Basket.Application;
using YGZ.Basket.Infrastructure;
using YGZ.Basket.Persistence;
using YGZ.BuildingBlocks.Messaging.MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddPresentationLayer()
    .AddApplicationLayer(builder.Configuration)
    .AddPersistenceLayer(builder.Configuration)
    .AddInfrastructureLayer(builder.Configuration)
    .AddMessageBrokerExtensions(builder.Configuration);


builder.Host.AddSerilogExtension(builder.Configuration);

builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("BasketDb")!)
        .AddRedis(builder.Configuration.GetConnectionString("Redis")!);


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

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
