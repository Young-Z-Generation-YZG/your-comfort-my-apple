
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone.ValueObjects;
using YGZ.Catalog.Domain.Tenants.ValueObjects;
using YGZ.Catalog.Infrastructure.Persistence.Configurations.Serializers;

namespace YGZ.Catalog.Infrastructure.Persistence.Configurations;

public static class DependencyInjection
{
    public static IServiceCollection AddMongoDbConfigurations(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(typeof(CategoryId), new CategoryIdSerialization());
        BsonSerializer.RegisterSerializer(typeof(ReviewId), new ReviewIdSerialization());
        BsonSerializer.RegisterSerializer(typeof(ModelId), new ModelIdSerialization());
        BsonSerializer.RegisterSerializer(typeof(SKUId), new SKUIdSerialization());
        BsonSerializer.RegisterSerializer(typeof(Slug), new SlugSerialization());
        BsonSerializer.RegisterSerializer(typeof(SKUCode), new SKUCodeSerialization());

        return services;
    }
}
