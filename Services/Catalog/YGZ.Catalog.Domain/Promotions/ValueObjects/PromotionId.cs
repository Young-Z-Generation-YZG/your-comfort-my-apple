﻿

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Promotions.ValueObjects;

public class PromotionId : ValueObject
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Value { get; }
    public string ValueStr { get; }
    
    public PromotionId(ObjectId value)
    {
        Value = value;
        ValueStr = value.ToString();
    }

    public static PromotionId CreateNew()
    {
        return new(ObjectId.GenerateNewId());
    }

    public static PromotionId? ToObjectId(string? objectId)
    {
        try
        {
            return !string.IsNullOrEmpty(objectId) ? new(ObjectId.Parse(objectId)) : null;
        } catch
        {
            return null;
        }
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}