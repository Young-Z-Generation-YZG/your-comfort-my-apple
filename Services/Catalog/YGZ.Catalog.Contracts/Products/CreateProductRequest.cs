namespace YGZ.Catalog.Contracts.Products;

public sealed record CreateProductRequest(string Name,
                                          string? Description,
                                          AverageRatingRequest? Average_rating,
                                          List<ImageRequest>? Images,
                                          List<ProductItemRequest>? Product_items,
                                          string? Category_id, 
                                          string? Promotion_id) { }

public sealed record ImageRequest(string Url, string Id) { }

public sealed record AverageRatingRequest(double Value, int NumRatings) { }

public sealed record ProductItemRequest(string Model,
                                        string Color,
                                        int Storage,
                                        double Price,
                                        int Quantity_in_stock,
                                        List<ImageRequest> Images) { }

