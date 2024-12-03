using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Api.Common.SwaggerExamples.Producs;

public class CreateProductRequestExample : IExamplesProvider<CreateProductRequest>
{
    public CreateProductRequest GetExamples()
    {
        var models = new List<ModelRequest> { new("iPhone 16", 1), new("iPhone 16 Plus", 2) };
        var colors = new List<ColorRequest>
        {
            new("ultramarine", "#a1aff5", 1),
            new("teal", "#bad7d6", 2),
            new("pink", "#e9aed6", 3),
            new("white", "#fafafa", 4),
            new("black", "#474a4d", 5)
        };
        var storages = new List<int> { 128, 256, 512 };
        var images = new List<ImageRequest>
        {
            new("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-1-202409_GEO_US?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127697", "image_id1", 1),
            new("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-2-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127558", "image_id2", 2),
            new("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-3-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127642", "image_id3", 3),
            new("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-4-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127808", "image_id4", 4),
            new("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-5-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127726", "image_id5", 5),
            new("https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/iphone16-digitalmat-gallery-6-202409?wid=728&hei=666&fmt=p-jpg&qlt=95&.v=1723669127802", "image_id6", 6)
        };



        return new CreateProductRequest(
            "iPhone 16",
            "iPhone 16 description",
            models,
            colors,
            storages,
            images,
            "672cdaed4e67692dff64a47c",
            "672cdaed4e67692dff64a47c");
    }
}
