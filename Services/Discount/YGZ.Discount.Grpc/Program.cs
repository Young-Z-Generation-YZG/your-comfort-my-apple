using YGZ.Discount.Application;
using YGZ.Discount.Application.Services;
using YGZ.Discount.Grpc;
using YGZ.Discount.Grpc.Services;
using YGZ.Discount.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

services
    .AddPresentationLayer()
    .AddApplicationLayer(builder.Configuration)
    .AddInfrastructureLayer(builder.Configuration);

var app = builder.Build();

app.MapGrpcReflectionService();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
