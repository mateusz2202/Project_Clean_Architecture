using Microsoft.EntityFrameworkCore;
using OperationAPI.Data;
using OperationAPI.Entities;
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

}
