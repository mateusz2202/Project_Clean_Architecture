using Operation.Application.Contracts.Repositories;
using System.Collections;
using Operation.Domain.Contracts;
using Operation.Application.Contracts.Services;

namespace Operation.Persistence.Repositories;

public class UnitOfWork<TId> : IUnitOfWork<TId>
{
    private readonly OperationDbContext _dbContext;
    private bool disposed;
    private Hashtable _repositories;
    private readonly ICacheService _cache;

    public UnitOfWork(OperationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _cache = cache;
    }

    public IRepositoryAsync<TEntity, TId> Repository<TEntity>() where TEntity : AuditableEntity<TId>
    {
        _repositories ??= new Hashtable();

        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryAsync<,>);

            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TId)), _dbContext);

            _repositories.Add(type, repositoryInstance);
        }

        return (IRepositoryAsync<TEntity, TId>)_repositories[type];
    }

    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
    {
        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        var tasks = new List<Task>();
        foreach (var cacheKey in cacheKeys)
        {
            tasks.Add(_cache.RemoveAsync(cacheKey));
        }
        await Task.WhenAll(tasks);
        return result;
    }

    public Task Rollback()
    {
        _dbContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                //dispose managed resources
                _dbContext.Dispose();
            }
        }
        //dispose unmanaged resources
        disposed = true;
    }
}
