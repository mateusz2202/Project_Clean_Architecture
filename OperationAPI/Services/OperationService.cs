using OperationAPI.Data;
using OperationAPI.Interfaces;

namespace OperationAPI.Services;

public class OperationService : IOperationService
{
    private readonly OperationDbContext _dbContext;
    public OperationService(OperationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddOperation(string name, string code)
    {
        await _dbContext.Operations.AddAsync(new Entities.Operation() { Name = name, Code = code });
        xd

        await _dbContext.SaveChangesAsync();
        await Task.CompletedTask;
    }
}
