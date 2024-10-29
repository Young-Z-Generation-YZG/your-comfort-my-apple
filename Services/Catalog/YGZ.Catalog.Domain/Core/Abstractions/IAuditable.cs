using MongoDB.Bson.Serialization.Attributes;

namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface IAuditable
{ 
    public DateTime Created_at { get; }

    public DateTime Updated_at { get; set; }
}
