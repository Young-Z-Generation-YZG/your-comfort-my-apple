

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using YGZ.Catalog.Domain.Core.Abstractions.Data;
using YGZ.Catalog.Persistence.Configurations;

namespace YGZ.Catalog.Persistence.Data;

public class MongoContext : IMongoContext
{
    private IClientSessionHandle Session { get; set; }
    private readonly List<Func<Task>> _commands = new();
    private readonly IConfiguration _configuration;
    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDb;

    public MongoContext(IOptions<CatalogDbSetting> options)
    {
        _mongoClient = new MongoClient(options.Value.ConnectionString);
        _mongoDb = _mongoClient.GetDatabase(options.Value.DatabaseName);
    }
    public async Task<int> SaveChanges()
    {
        var commandTasks = _commands.Select(c => c());

        await Task.WhenAll(commandTasks);
        //using (Session = await _mongoClient.StartSessionAsync())
        //{
        //    //Session.StartTransaction();

        //    //try
        //    //{
        //    //    var commandTasks = _commands.Select(c => c());

        //    //    await Task.WhenAll(commandTasks);

        //    //    await Session.CommitTransactionAsync();
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    Console.WriteLine("Error writing to MongoDB: " + e.Message);

        //    //    await Session.AbortTransactionAsync();

        //    //    return 0;
        //    //}

        //    var commandTasks = _commands.Select(c => c());

        //    await Task.WhenAll(commandTasks);

        //    //await Session.CommitTransactionAsync();
        //}

        return _commands.Count;
    }

    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _mongoDb.GetCollection<T>(name);
    }

    public void Dispose()
    {
        Session?.Dispose();
        GC.SuppressFinalize(this);
    }

    public void AddCommand(Func<Task> func)
    {
        _commands.Add(func);
    }
}
