

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Domain.Products.Iphone16;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;

namespace YGZ.Catalog.Infrastructure.Persistence.Extensions;

public static class SeedDataExtensions
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var iPhone16ModelRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<IPhone16Model>>();
        var iPhone16DetailRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<IPhone16Detail>>();


        await SeedIPhone16ModelAsync(iPhone16ModelRepository);
        await SeedIPhone16DetailsAsync(iPhone16DetailRepository);
    }

    private static async Task SeedIPhone16ModelAsync(IMongoRepository<IPhone16Model> iPhone16ModelRepository)
    {
        var existingItems = await iPhone16ModelRepository.GetAllAsync();

        if (existingItems.Count == 0)
        {
            foreach (var item in SeedData.IPhone16Models)
            {
                await iPhone16ModelRepository.InsertOneAsync(item);
            }
        }
    }

    private static async Task SeedIPhone16DetailsAsync(IMongoRepository<IPhone16Detail> iPhone16DetailRepository)
    {
        var existingItems = await iPhone16DetailRepository.GetAllAsync();

        if (existingItems.Count == 0)
        {
            foreach (var item in SeedData.IPhone16Details)
            {
                await iPhone16DetailRepository.InsertOneAsync(item);
            }
        }
    }
}
