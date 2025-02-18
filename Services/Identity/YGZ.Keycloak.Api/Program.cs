using Keycloak.AuthServices.Authorization;
using YGZ.Keycloak.Api.Extensions;
using YGZ.Keycloak.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add Layers
services.AddInfrastructureLayer(builder.Configuration);

// Add services to the container.
services.AddProblemDetails();
services.AddSwaggerExtensions();

services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());


services.AddControllers(options => options.AddProtectedResources());

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseStatusCodePages();
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(ui => ui.UseApplicationSwaggerSettings(builder.Configuration));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
