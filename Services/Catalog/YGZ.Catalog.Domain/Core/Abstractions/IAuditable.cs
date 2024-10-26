namespace YGZ.Catalog.Domain.Core.Abstractions;

public interface IAuditable
{
    public DateTime Created_at { get; set; }

    public DateTime Updated_at { get; set; }
}
