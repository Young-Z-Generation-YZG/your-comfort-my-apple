

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using YGZ.Catalog.Application.Abstractions.Data;
using YGZ.Catalog.Domain.Categories;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone16.Entities;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Infrastructure.Persistence.Extensions;

public static class SeedDataExtensions
{
    public static async Task ApplySeedDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        //var iPhone16ModelRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<IPhone16Model, IPhone16ModelId>>();
        var iPhone16DetailRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<IPhone16Detail, IPhone16Id>>();
        var categoryRepository = scope.ServiceProvider.GetRequiredService<IMongoRepository<Category, CategoryId>>();


        await SeedCategoriesAsync(categoryRepository);

        //await SeedIPhone16ModelAsync(iPhone16ModelRepository);

        await SeedIPhone16DetailsAsync(iPhone16DetailRepository);
    }

    private static async Task SeedCategoriesAsync(IMongoRepository<Category, CategoryId> categoryRepository)
    {
        var existingItems = await categoryRepository.GetAllAsync();

        if (existingItems.Count == 0)
        {
            foreach (var item in Seeds.Seeds.Categories)
            {
                await categoryRepository.InsertOneAsync(item);
            }
        }
    }

    //private static async Task SeedIPhone16ModelAsync(IMongoRepository<IPhone16Model, IPhone16ModelId> iPhone16ModelRepository)
    //{
    //    var existingItems = await iPhone16ModelRepository.GetAllAsync();

    //    if (existingItems.Count == 0)
    //    {
    //        foreach (var item in Seeds.Seeds.IPhone16_16Plus_Models)
    //        {
    //            await iPhone16ModelRepository.InsertOneAsync(item);
    //        }
    //        foreach (var item in Seeds.Seeds.IPhone16e_Models)
    //        {
    //            await iPhone16ModelRepository.InsertOneAsync(item);
    //        }
    //        foreach (var item in Seeds.Seeds.IPhone15_15Plus_Models)
    //        {
    //            await iPhone16ModelRepository.InsertOneAsync(item);
    //        }
    //    }
    //}

    private static async Task SeedIPhone16DetailsAsync(IMongoRepository<IPhone16Detail, IPhone16Id> iPhone16DetailRepository)
    {
        //var existingItems = await iPhone16DetailRepository.GetAllAsync();

        //if (existingItems.Count == 0)
        //{
        //    foreach (var item in Seeds.Seeds.IPhone16_16Plus_Details)
        //    {
        //        await iPhone16DetailRepository.InsertOneAsync(item);
        //    }
        //    foreach (var item in Seeds.Seeds.IPhone16e_Details)
        //    {
        //        await iPhone16DetailRepository.InsertOneAsync(item);
        //    }
        //    foreach (var item in Seeds.Seeds.IPhone15_Details)
        //    {
        //        await iPhone16DetailRepository.InsertOneAsync(item);
        //    }
        //}
    }
}
