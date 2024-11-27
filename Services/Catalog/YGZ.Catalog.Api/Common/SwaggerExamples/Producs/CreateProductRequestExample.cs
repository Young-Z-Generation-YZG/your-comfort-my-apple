using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Api.Common.SwaggerExamples.Producs;

public class CreateProductRequestExample : IExamplesProvider<CreateProductRequest>
{
    public CreateProductRequest GetExamples()
    {
        var images = new List<ImageRequest> { new("image_url", "image_id") };
        var colors = new List<ColorRequest> { new("color1", "color_hash1", "image_url1", 1) };
        var models = new List<ModelRequest> { new("model1", 1), new("model2", 2) };


        var storages = new List<int> { 64, 128, 256 };

        return new CreateProductRequest(
            "iPhone 16",
            "iPhone 16 description",
            images,
            models,
            colors,
            storages,
            "672cdaed4e67692dff64a47c",
            "672cdaed4e67692dff64a47c");
    }
}
