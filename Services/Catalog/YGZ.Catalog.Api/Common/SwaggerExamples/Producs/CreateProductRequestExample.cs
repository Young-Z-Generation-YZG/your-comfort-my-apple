using FluentValidation;
using Swashbuckle.AspNetCore.Filters;
using YGZ.Catalog.Contracts.Common;
using YGZ.Catalog.Contracts.Products;

namespace YGZ.Catalog.Api.Common.SwaggerExamples.Producs;

public class CreateProductRequestExample : IExamplesProvider<CreateProductRequest>
{
    public CreateProductRequest GetExamples()
    {
        var averageRating = new AverageRatingRequest(0, 0);
        var images = new List<ImageRequest> { new("image_url", "image_id") };
        var models = new List<string> { "model1", "model2" };
        var colors = new List<string> { "color1", "color2" };

        return new CreateProductRequest(
            "iPhone 16",
            "iPhone 16 description",
            averageRating,
            images,
            models,
            colors,
            "672cdaed4e67692dff64a47c",
            "672cdaed4e67692dff64a47c");
    }
}
