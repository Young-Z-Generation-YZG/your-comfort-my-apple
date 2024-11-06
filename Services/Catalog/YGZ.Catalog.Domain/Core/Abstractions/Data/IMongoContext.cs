
using MongoDB.Driver;

namespace YGZ.Catalog.Domain.Core.Abstractions.Data;

public interface IMongoContext : IDisposable
{
    void AddCommand(Func<Task> func);
    Task<int> SaveChanges();
    IMongoCollection<T> GetCollection<T>(string name);
}

