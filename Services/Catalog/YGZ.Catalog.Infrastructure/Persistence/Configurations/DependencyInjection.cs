
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ValueObjects;
using YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(typeof(ProductId), new ProductIdSerialization());
        BsonSerializer.RegisterSerializer(typeof(ProductItemId), new ProductItemIdSerialization());
        BsonSerializer.RegisterSerializer(typeof(CategoryId), new CategoryIdSerialization());
        BsonSerializer.RegisterSerializer(typeof(Slug), new SlugSerialization());


        return services;
    }
}
