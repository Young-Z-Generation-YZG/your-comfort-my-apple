
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Domain.Products;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand : ICommand<Product> {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    //public IFormFile[] Files { get; set; } = [];
    public List<ImageCommand> Images { get; set; } = new();
    public AverageRatingCommand AverageRating { get; set; } = new(0, 0);
    public List<ProductItemCommand> ProductItems { get; set; } = new();

    public string CategoryId { get; set; } = null!;
    public string PromotionId { get; set; } = null!;
}

public sealed record ImageCommand(string Url, string Id) { }

public sealed record AverageRatingCommand(double Value, int NumRatings) {}

public sealed record ProductItemCommand(string Model,
                                        string Color,
                                        int Storage,
                                        double Price,
                                        int Quantity_in_stock,
                                        List<ImageCommand> Images) { }

