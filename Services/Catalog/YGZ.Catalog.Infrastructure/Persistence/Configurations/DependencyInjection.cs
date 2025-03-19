
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;
using YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddMongoDbConfigurations(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(typeof(CategoryId), new CategoryIdSerialization());

        BsonSerializer.RegisterSerializer(typeof(IPhone16Id), new IPhone16IdSerialization());
        BsonSerializer.RegisterSerializer(typeof(IPhone16ModelId), new IPhone16ModelIdSerialization());

        BsonSerializer.RegisterSerializer(typeof(Slug), new SlugSerialization());


        return services;
    }
}
