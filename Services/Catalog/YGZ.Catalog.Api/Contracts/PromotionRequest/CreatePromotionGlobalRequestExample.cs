using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using NJsonSchema.Generation;

namespace YGZ.Catalog.Api.Contracts.PromotionRequest;

public class CreatePromotionGlobalRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(CreatePromotionGlobalRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                global_title = "Black Friday for categories",
                global_description = "Get 50% off on all items during the Black Friday Sale!",
                global_type = "CATEGORIES",
                event_id = "f55f322f-6406-4dfa-b2ea-2777f7813e70",
                promotion_categories = new List<object>
                {
                    new
                    {
                        category_id = "category_id",
                        category_name = "iPhone 16",
                        category_slug = "iphone-16",
                        discount_type = "PERCENTAGE",
                        discount_value = 0.05
                    }
                },
                promotion_products = new List<object>
                {
                    new
                    {
                        product_id = "product_id",
                        product_slug = "iphone-16-128gb",
                        product_image = "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US",
                        discount_type = "PERCENTAGE",
                        discount_value = 0.05
                    },
                    new
                    {
                        product_id = "product_id",
                        product_slug = "iphone-16-256gb",
                        product_image = "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US",
                        discount_type = "PERCENTAGE",
                        discount_value = 0.1
                    }
                }
            };
        }
    }
}