using Document.Application.Interfaces.Repositories;
using Document.Domain.Contracts;
using LazyCache;
using System.Collections;


namespace Document.Persistence.Repositories;

public class ExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> : IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> where TEntity : AuditableEntity<TEntityId>
{  
    private readonly DocumentDbContext _dbContext;
    private bool _disposed;
    private Hashtable _repositories;
    private readonly IAppCache _cache;

    public ExtendedAttributeUnitOfWork(DocumentDbContext dbContext, IAppCache cache)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));  
        _cache = cache;
    }

    public IRepositoryAsync<T, TId> Repository<T>() where T : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>
    {
        if (_repositories == null)
            _repositories = new Hashtable();

        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(RepositoryAsync<,>);

            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T), typeof(TId)), _dbContext);

            _repositories.Add(type, repositoryInstance);
        }

        return (IRepositoryAsync<T, TId>)_repositories[type];
    }

    public async Task<int> Commit(CancellationToken cancellationToken)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CommitAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
    {
        var result = await _dbContext.SaveChangesAsync(cancellationToken);
        foreach (var cacheKey in cacheKeys)
        {
            _cache.Remove(cacheKey);
        }
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
        if (!_disposed)
        {
            if (disposing)
            {
                //dispose managed resources
                _dbContext.Dispose();
            }
        }
        //dispose unmanaged resources
        _disposed = true;
    }
}
