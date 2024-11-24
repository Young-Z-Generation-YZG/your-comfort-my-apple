

using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Core.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Products;
using YGZ.Catalog.Domain.Products.ValueObjects;
using YGZ.Catalog.Domain.Promotions.ValueObjects;
using YGZ.Catalog.Persistence.Configurations.Categories;
using YGZ.Catalog.Persistence.Configurations.Common;
using YGZ.Catalog.Persistence.Configurations.Products;
using YGZ.Catalog.Persistence.Configurations.Promotions;

namespace YGZ.Catalog.Persistence.Configurations;

public static class Configurations 
{
    public static IServiceCollection AddMongoDbConfiguration(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(typeof(Slug), new SlugSerializer());
        BsonSerializer.RegisterSerializer(typeof(SKU), new SKUSerializer());
        BsonSerializer.RegisterSerializer(typeof(ProductStateEnum), new SmartEnumSerializer<ProductStateEnum>());

        BsonSerializer.RegisterSerializer(typeof(ProductId), new ProductIdSerialzer());
        BsonSerializer.RegisterSerializer(typeof(ProductItemId), new ProductItemIdSerialzer());
        BsonSerializer.RegisterSerializer(typeof(CategoryId), new CategoryIdSerializer());
        BsonSerializer.RegisterSerializer(typeof(PromotionId), new PromotionIdSerialzier());

        //BsonSerializer.RegisterSerializer(typeof(DateTime), new LocalDateTimeSerializer());



        return services;
    }
}
