using NJsonSchema.Generation;

namespace YGZ.Basket.Api.Contracts;

public class StoreEventItemRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(StoreEventItemRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                event_item_id = "04edf970-b569-44ac-a116-9847731929ab",
                product_information = new
                {
                    product_name = "iPhone 15 128GB",
                    model_normalized_name = "IPHONE_15",
                    color_normalized_name = "BLUE",
                    storage_normalized_name = "128GB",
                    display_image_url = "https://res.cloudinary.com/delkyrtji/image/upload/v1744960327/iphone-15-finish-select-202309-6-1inch-blue_zgxzmz.webp",
                }
            };
        }
    }
}