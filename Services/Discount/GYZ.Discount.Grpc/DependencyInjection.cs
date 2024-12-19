using GYZ.Discount.Grpc.Abstractions;
using GYZ.Discount.Grpc.Infrastructure;

namespace GYZ.Discount.Grpc;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
        services.AddSingleton<IUniqueCodeGenerator, CodeUniqueGenerator>();


        return services;
    }
}
