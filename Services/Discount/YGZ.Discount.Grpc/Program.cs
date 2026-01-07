using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.FeatureManagement;
using YGZ.Discount.Application;
using YGZ.Discount.Grpc;
using YGZ.Discount.Grpc.GrpcServices;
using YGZ.Discount.Infrastructure;
using YGZ.Discount.Infrastructure.Persistence.Extensions;

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

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

services
    .AddPresentationLayer(builder)
    .AddApplicationLayer(builder.Configuration)
    .AddInfrastructureLayer(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationAsync();

    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    
    // Seed data initialization - wrap in try-catch to prevent startup failure
    try
    {
        // Wait a bit to ensure MassTransit bus and other services' consumers are ready
        // This gives time for Catalog service to register its consumers
        logger.LogInformation("Waiting for mongo setup and consumers to be ready before seeding...");
        await Task.Delay(TimeSpan.FromSeconds(120), lifetime.ApplicationStopping);
        
        await app.ApplySeedDataAsync();
        
        logger.LogInformation("Seed data applied successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to apply seed data. Application will continue without seed data.");
    }
}

app.MapGrpcReflectionService();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
