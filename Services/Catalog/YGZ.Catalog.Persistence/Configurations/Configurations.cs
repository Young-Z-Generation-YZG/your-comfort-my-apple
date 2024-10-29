

using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.ValueObjects;
using YGZ.Catalog.Persistence.Configurations.Common;
using YGZ.Catalog.Persistence.Configurations.Products;

namespace YGZ.Catalog.Persistence.Configurations;

public static class Configurations 
{
    public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services)
    {

        BsonSerializer.RegisterSerializer(typeof(Slug), new SlugSerializer());
        BsonSerializer.RegisterSerializer(typeof(ProductId), new ProductIdSerialzer());
        BsonSerializer.RegisterSerializer(typeof(DateTime), new LocalDateTimeSerializer());

        return services;
    }
}
