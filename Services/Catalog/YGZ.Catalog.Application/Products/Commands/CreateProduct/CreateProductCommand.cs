
using YGZ.Catalog.Application.Common.Commands;
using YGZ.Catalog.Application.Core.Abstractions.Messaging;
using YGZ.Catalog.Domain.Products;

namespace YGZ.Catalog.Application.Products.Commands.CreateProduct;

public sealed record CreateProductCommand : ICommand<bool> {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<ImageCommand> Images { get; set; }
    public AverageRatingCommand AverageRating { get; set; }
    public List<string> Models { get; set; }
    public List<string> Colors { get; set; }
    public string CategoryId { get; set; }
    public string PromotionId { get; set; }
}
