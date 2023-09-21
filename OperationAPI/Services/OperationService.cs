using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using OperationAPI.Data;
using OperationAPI.Entities;
using OperationAPI.Exceptions;
using OperationAPI.Interfaces;

namespace OperationAPI.Services;

public class OperationService : IOperationService
{
    private const string OPERATION_KEY = "operation";

    private readonly OperationDbContext _dbContext;
    private readonly ICacheService _cacheService;

    public OperationService(OperationDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<Operation>> GetAll()
    {
        var cacheData = await _cacheService.GetAsync<IEnumerable<Operation>>(OPERATION_KEY);

        if (cacheData != null && cacheData.Count() > 0)
            return cacheData;

        var result = await _dbContext.Operations.ToListAsync();

        var expiryTime = DateTimeOffset.Now.AddMinutes(30);
        await _cacheService.SetAsync(OPERATION_KEY, result, expiryTime);

        return result;
    }

    public async Task AddOperation(string name, string code)
    {
        var addedObj = await _dbContext.Operations.AddAsync(new Entities.Operation() { Name = name, Code = code });
        await _dbContext.SaveChangesAsync();
        await _cacheService.RemoveAsync(OPERATION_KEY);    
        await Task.CompletedTask;
    }

    public async Task DeleteOperation(string code)
    {
        var operation = await _dbContext.Operations.FirstOrDefaultAsync(o => o.Code == code)
                        ?? throw new NotFoundException("Operation not found");

        _dbContext.Operations.Remove(operation);
        await _dbContext.SaveChangesAsync();
        await _cacheService.RemoveAsync(OPERATION_KEY);
        await Task.CompletedTask;
    }


    public async Task DeleteAll()
    {
        _dbContext.Operations.ExecuteDelete();
        await _dbContext.SaveChangesAsync();
        await _cacheService.RemoveAsync(OPERATION_KEY);
        await Task.CompletedTask;
    }
}
