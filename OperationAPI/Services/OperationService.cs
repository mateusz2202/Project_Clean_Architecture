using Microsoft.EntityFrameworkCore;
using OperationAPI.Data;
using OperationAPI.Entities;
using OperationAPI.Exceptions;
using OperationAPI.Interfaces;

namespace OperationAPI.Services;

public class OperationService : IOperationService
{
    private readonly OperationDbContext _dbContext;
    public OperationService(OperationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Operation>> GetAll()
    {
        var result = await _dbContext.Operations.ToListAsync();
        return result;
    }

    public async Task AddOperation(string name, string code)
    {
        await _dbContext.Operations.AddAsync(new Entities.Operation() { Name = name, Code = code });
        await _dbContext.SaveChangesAsync();
        await Task.CompletedTask;
    }

    public async Task DeleteOperation(string code)
    {
        var operation = await _dbContext.Operations.FirstOrDefaultAsync(o => o.Code == code)
                        ?? throw new NotFoundException("Operation not found");

        _dbContext.Operations.Remove(operation);
        await _dbContext.SaveChangesAsync();
        await Task.CompletedTask;
    }

    public async Task DeleteAll()
    {
        _dbContext.Operations.ExecuteDelete();
        await _dbContext.SaveChangesAsync();
        await Task.CompletedTask;
    }
}
