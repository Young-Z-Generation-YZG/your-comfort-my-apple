using MongoDB.Bson.Serialization.Attributes;
using YGZ.BuildingBlocks.Shared.Enums;
using YGZ.Catalog.Domain.Categories.ValueObjects;
using YGZ.Catalog.Domain.Common.ValueObjects;
using YGZ.Catalog.Domain.Core.Abstractions;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;
using YGZ.Catalog.Domain.Products.Common.ValueObjects;
using YGZ.Catalog.Domain.Products.Iphone16.ValueObjects;

namespace YGZ.Catalog.Domain.Products.Iphone16.Entities;

[BsonCollection("IPhone16Details")]
public class IPhone16Detail : Entity<IPhone16Id>, IAuditable, ISoftDelete
{
    public IPhone16Detail(IPhone16Id id) : base(id)
    {

    }

    [BsonElement("general_model")]
    required public string GeneralModel { get; set; }

    [BsonElement("model")]
    required public string Model { get; set; }

    [BsonElement("color")]
    required public Color Color { get; set; }

    [BsonElement("storage")]
    required public Storage Storage { get; set; }

    [BsonElement("unit_price")]
    required public decimal UnitPrice { get; set; } = 0;

    [BsonElement("description")]
    public string Description { get; set; } = default!;

    [BsonElement("name_tag")]
    public ProductNameTag ProductNameTag { get; set; } = ProductNameTag.IPHONE;

    [BsonElement("available_in_stock")]
    public int AvailableInStock { get; set; } = 0;

    [BsonElement("total_sold")]
    public int TotalSold { get; set; } = 0;

    [BsonElement("state")]
    public State State { get; set; } = State.INACTIVE;

    [BsonElement("images")]
    public List<Image> Images { get; set; } = [];

    [BsonElement("slug")]
    required public Slug Slug { get; set; } = default!;

    [BsonElement("iphone_model_id")]
    required public IPhone16ModelId IPhoneModelId { get; set; }

    [BsonElement("category_id")]
    required public CategoryId CategoryId { get; set; }

    [BsonElement("created_at")]
    public DateTime CreatedAt => Id.Id?.CreationTime ?? DateTime.Now;

    [BsonElement("updated_at")]
    public DateTime UpdatedAt => Id.Id?.CreationTime ?? DateTime.Now;

    [BsonElement("modified_by")]
    public Guid? ModifiedBy => null;

    [BsonElement("is_deleted")]
    public bool IsDeleted => false;

    [BsonElement("deleted_at")]
    public DateTime? DeletedAt => null;

    [BsonElement("deleted_by")]
    public Guid? DeletedBy => null;

    public static IPhone16Detail Create(string model,
                                        Color color,
                                        int storage,
                                        decimal unitPrice,
                                        string description,
                                        List<Image> images,
                                        string iPhoneModelId,
                                        string categoryId)
    {
        var storageEnum = Storage.FromValue(storage);

        return new IPhone16Detail(IPhone16Id.Of(iPhoneModelId))
        {
            GeneralModel = Slug.Create(model).Value,
            Model = model,
            Color = color,
            Storage = storageEnum,
            UnitPrice = unitPrice,
            Description = description,
            AvailableInStock = 0,
            TotalSold = 0,
            Images = images,
            Slug = Slug.Create(model),
            IPhoneModelId = IPhone16ModelId.Of(iPhoneModelId),
            CategoryId = CategoryId.Of(categoryId)
        };
    }
}
