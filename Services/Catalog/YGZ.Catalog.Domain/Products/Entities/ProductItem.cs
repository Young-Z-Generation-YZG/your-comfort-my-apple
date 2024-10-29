
//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
//using YGZ.Catalog.Domain.Core.Base;

//namespace YGZ.Catalog.Domain.Products.Entities;

//public class ProductItem : BaseEntity
//{
//    public string Model { get; set; } = default!;
//    public string Size { get; set; } = default!;
//    public string Color { get; set; } = default!;
//    public int Storage { get; set; } = 0;
//    public decimal Price { get; set; } = 0;
//    public int Quantity_in_stock { get; set; } = 0;

//    [BsonRepresentation(BsonType.ObjectId)]
//    public string Product_id { get; set; } = default!;
//    public Product Product { get; set; } = default!;
//}
