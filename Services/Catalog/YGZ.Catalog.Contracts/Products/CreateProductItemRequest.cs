

using YGZ.Catalog.Contracts.Common;

namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductItemRequest(string Model,
                                              string Color,
                                              int Storage,
                                              double Price,
                                              int Quantity_in_stock,
                                              List<ImageRequest> Images, 
                                              string Product_id) { }