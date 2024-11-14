
using YGZ.Catalog.Application.Common.Commands;
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Domain.Products;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand : ICommand<Product> {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    //public IFormFile[] Files { get; set; } = [];
    public List<ImageCommand> Images { get; set; } = new();
    public AverageRatingCommand AverageRating { get; set; } = new(0, 0);
    public List<string> Models { get; set; } = new();
    public List<string> Colors { get; set; } = new();
    public string CategoryId { get; set; } = null!;
    public string PromotionId { get; set; } = null!;
}
