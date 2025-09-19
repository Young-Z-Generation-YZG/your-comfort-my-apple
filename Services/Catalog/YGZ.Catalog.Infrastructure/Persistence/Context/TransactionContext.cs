using MongoDB.Driver;
using YGZ.Catalog.Application.Abstractions.Data.Context;

namespace YGZ.Catalog.Infrastructure.Persistence.Context;

public class TransactionContext : ITransactionContext
{
    public IClientSessionHandle? CurrentSession { get; private set; }

    public void SetSession(IClientSessionHandle session)
    {
        CurrentSession = session;
    }

    public void ClearSession()
    {
        CurrentSession = null;
    }
}
