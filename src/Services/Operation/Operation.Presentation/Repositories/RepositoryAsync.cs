using Microsoft.EntityFrameworkCore;
using Operation.Application.Contracts.Repositories;
using Operation.Domain.Contracts;
using System.Linq.Expressions;

namespace Operation.Persistence.Repositories;

public class RepositoryAsync<T, TId> : IRepositoryAsync<T, TId> where T : AuditableEntity<TId>
{
    private readonly OperationDbContext _dbContext;

    public RepositoryAsync(OperationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> Entities => _dbContext.Set<T>();

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>().AddAsync(entity);
        return entity;
    }

    public Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Expression<Func<T, bool>> condition)
    {
        _dbContext.Set<T>().Where(condition).ExecuteDelete();
        return Task.CompletedTask;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbContext
            .Set<T>()
            .ToListAsync();
    }

    public async Task<T> GetByIdAsync(TId id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<List<T>> GetPagedResponseAsync(int pageNumber, int pageSize)
    {
        return await _dbContext
            .Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        T? exist = await _dbContext.Set<T>().FindAsync(entity.Id);
        _dbContext.Entry(exist).CurrentValues.SetValues(entity);
    }

}