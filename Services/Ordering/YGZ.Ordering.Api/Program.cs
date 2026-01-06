using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using HealthChecks.UI.Client;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.FeatureManagement;
using YGZ.Ordering.Api;
using YGZ.Ordering.Api.Extensions;
using YGZ.Ordering.Application;
using YGZ.Ordering.Infrastructure;
using YGZ.Ordering.Infrastructure.Persistence.Extensions;

// Helper method to create self-signed certificate for Docker
static X509Certificate2 CreateSelfSignedCertificate()
{
    var subjectName = "CN=localhost";
    using var rsa = RSA.Create(2048);
    var request = new CertificateRequest(subjectName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, false));
    request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension([new Oid("1.3.6.1.5.5.7.3.1")], false));

    var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
    return new X509Certificate2(certificate.Export(X509ContentType.Pfx, "password"), "password", X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
}

var builder = WebApplication.CreateBuilder(args);

// Add Feature Management early to check feature flags before Kestrel configuration
builder.Services.AddFeatureManagement(builder.Configuration.GetSection("FeatureManagement"));

// Build temporary service provider to access IFeatureManager
// Note: This is necessary to check feature flags before Kestrel configuration.
// The temporary service provider is disposed immediately after use.
#pragma warning disable ASP0000 // Do not call 'IServiceCollection.BuildServiceProvider' in application code
using var tempServiceProvider = builder.Services.BuildServiceProvider();
#pragma warning restore ASP0000
var featureManager = tempServiceProvider.GetRequiredService<IFeatureManager>();

// Check if self-signed SSL is enabled via feature flag
var enableSelfSsl = await featureManager.IsEnabledAsync("EnableSelfSsl");

if (enableSelfSsl)
{
    // Configure Kestrel for HTTP and HTTPS
    builder.WebHost.ConfigureKestrel(options =>
    {
        // HTTP endpoint with HTTP/2 support for gRPC
        options.ListenAnyIP(8080, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        });

        // HTTPS endpoint with HTTP/2 (only if EnableSelfSsl feature is enabled)
        options.ListenAnyIP(8081, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            // Generate self-signed certificate for Docker
            var cert = CreateSelfSignedCertificate();
            listenOptions.UseHttps(cert);
        });

    });
}

var services = builder.Services;
var host = builder.Host;

// Add Layers
services
    .AddPresentationLayer(builder)
    .AddInfrastructureLayer(builder.Configuration)
    .AddApplicationLayer(builder.Configuration);

// Add services to the container.
services.AddProblemDetails();
services.AddSwaggerExtension();

services.ConfigureHttpClientDefaults(http => http.AddStandardResilienceHandler());


services.AddControllers(options => options.AddProtectedResources())
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.WriteIndented = true; // Optional
        });

builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("OrderingDb")!);

var app = builder.Build();

app.UseStatusCodePages();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(ui => ui.SwaggerOAuthSettings(builder.Configuration));

    await app.ApplyMigrationAsync();
    await app.ApplySeedDataAsync();
}

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseCors(options =>
{
    options.AllowAnyHeader()
           .AllowAnyOrigin()
           .AllowAnyMethod()
           .SetPreflightMaxAge(TimeSpan.FromHours(24 * 30 * 6));
});

//app.UseHttpsRedirection();
app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcReflectionService();
app.MapControllers();
// Add this line to map gRPC endpoints
app.MapGrpcEndpoints();

app.MapRazorPages();

app.Run();