using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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

// Configure Kestrel for HTTP and HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    // HTTP endpoint with HTTP/2 support for gRPC
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
    });
    
    // HTTPS endpoint with HTTP/2
    options.ListenAnyIP(8081, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        // Generate self-signed certificate for Docker
        var cert = CreateSelfSignedCertificate();
        listenOptions.UseHttps(cert);
    });
});
var services = builder.Services;

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

services
    .AddPresentationLayer()
    .AddApplicationLayer(builder.Configuration)
    .AddInfrastructureLayer(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationAsync();
    await app.ApplySeedDataAsync();
}

app.MapGrpcReflectionService();

// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
