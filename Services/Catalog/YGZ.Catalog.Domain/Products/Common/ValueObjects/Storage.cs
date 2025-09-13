using MongoDB.Bson.Serialization.Attributes;
using YGZ.Catalog.Domain.Core.Enums;
using YGZ.Catalog.Domain.Core.Primitives;

namespace YGZ.Catalog.Domain.Products.Common.ValueObjects;

public class Storage : ValueObject
{
    [BsonElement("name")]
    public string Name { get; init; }

    [BsonElement("value")]
    public int Value { get; init; }

    [BsonElement("order")]
    public int Order { get; init; } = 0;

    private Storage(EStorage storage, int order)
    {


        Name = storage.Name;
        Value = storage.Value;
        Order = order;
    }

    public static Storage Create(string name, int order)
    {
        EStorage.TryFromName(name, out var storage);

        if (storage is null)
        {
            throw new ArgumentException("Invalid EStorage ${name}", name);
        }

        return new Storage(storage, order);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Value;
        yield return Order;
    }
}
