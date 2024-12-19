using GYZ.Discount.Grpc;
using GYZ.Discount.Grpc.Data;
using GYZ.Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.Services.AddPresentationLayer();

builder.Services.AddDbContext<DiscountDbContext>(options =>
{
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    options.UseNpgsql(builder.Configuration.GetConnectionString("DiscountDb"));
});

var app = builder.Build();


IWebHostEnvironment env = app.Environment;
if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

await app.ApplyMigrationAsync();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
