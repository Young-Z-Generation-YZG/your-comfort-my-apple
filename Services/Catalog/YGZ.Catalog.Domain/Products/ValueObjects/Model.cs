﻿

using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.ValueObjects;

public class Model : ValueObject
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("order")]
    public int Order { get; set; }

    public Model(string name, int order)
    {
        Name = name;
        Order = order;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}