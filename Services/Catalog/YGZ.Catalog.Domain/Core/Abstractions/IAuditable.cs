using MongoDB.Bson.Serialization.Attributes;

namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface IAuditable
{ 
    public DateTime CreatedAt { get; }

    public DateTime UpdatedAt { get; set; }
}
