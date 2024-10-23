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
    .AddInfrastructureLayer();

builder.Host.AddSerilogExtension(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

app.Run();
