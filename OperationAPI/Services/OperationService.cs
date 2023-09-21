using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OperationAPI.Data;
using OperationAPI.Entities;
using OperationAPI.Exceptions;
using OperationAPI.Interfaces;
using OperationAPI.Models;
using System.Dynamic;


namespace OperationAPI.Services;

public class OperationService : IOperationService
{
    private const string OPERATION_KEY = "operation";
    private const string OPERATIONATTRIBUTE_KEY = "operationAttribute";
    private const string OPERATIONWITHATTRIBUTE_KEY = "operationWithAttribute";

    private readonly OperationDbContext _dbContext;
    private readonly CosmosClient _cosmosClient;
    private readonly ICacheService _cacheService;

    public OperationService(OperationDbContext dbContext, CosmosClient cosmosClient, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cosmosClient = cosmosClient;
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
    public async Task<IEnumerable<dynamic>> GetAllWithAtrributes()
    {
        var cacheData = await _cacheService.GetAsync<IEnumerable<dynamic>>(OPERATIONWITHATTRIBUTE_KEY);
        if (cacheData != null && cacheData.Count() > 0)
            return cacheData;

        var operations = await GetAll();
        var attributes = await GetAllAttribute();
        var dynamicAtrributes = attributes.Select(x => JsonConvert.DeserializeObject<dynamic>(x.ToString())).Select(x => (dynamic)x);
        
        var union = from x in operations
                     join z in dynamicAtrributes on x.Id equals (int?)z.id
                     select new { Operation = x, Atrributes = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(z)) };
        
        var result = union.ToList();

        var expiryTime = DateTimeOffset.Now.AddMinutes(30);
        await _cacheService.SetAsync(OPERATIONWITHATTRIBUTE_KEY, result, expiryTime);

        return result;
    }

    public async Task<Operation> Get(int id)
    {
        var operation = await _dbContext.Operations.FirstOrDefaultAsync(x => x.Id == id)
                         ?? throw new NotFoundException("Operation not found");
        return operation;
    }

    public async Task<Operation> AddOperation(CreateOperationDTO dto)
    {
        var operation = await _dbContext.Operations.AddAsync(new Entities.Operation() { Name = dto.Name, Code = dto.Code });
        await _dbContext.SaveChangesAsync();
        await _cacheService.RemoveAsync(OPERATIONATTRIBUTE_KEY);     
        await _cacheService.RemoveAsync(OPERATIONWITHATTRIBUTE_KEY);
        return operation.Entity;
    }

    public async Task AddOperationWithAttributes(CreateOperationWithAttributeDTO dto)
    {
        var addObj = await AddOperation(dto.CreateOperationDTO);
        var item = JsonConvert.DeserializeObject<dynamic>(dto?.Attributes?.ToString());
        ((dynamic)item).id = addObj.Id.ToString();
        ((dynamic)item).code = addObj.Code;

        string json = JsonConvert.SerializeObject(item);
        await AddAttributes(json);
        await _cacheService.RemoveAsync(OPERATIONATTRIBUTE_KEY);
        await _cacheService.RemoveAsync(OPERATION_KEY);
        await _cacheService.RemoveAsync(OPERATIONWITHATTRIBUTE_KEY);
        await Task.CompletedTask;
    }

    public async Task DeleteOperation(string code)
    {
        var operation = await _dbContext.Operations.FirstOrDefaultAsync(o => o.Code == code)
                        ?? throw new NotFoundException("Operation not found");

        _dbContext.Operations.Remove(operation);
        await _dbContext.SaveChangesAsync();

        var container = await GetContainer();
        await container.DeleteItemAsync<object>($"{operation.Id}", new PartitionKey(operation.Code));

        await _cacheService.RemoveAsync(OPERATIONATTRIBUTE_KEY);
        await _cacheService.RemoveAsync(OPERATION_KEY);
        await _cacheService.RemoveAsync(OPERATIONWITHATTRIBUTE_KEY);

        await Task.CompletedTask;
    }


    public async Task DeleteAll()
    {
        _dbContext.Operations.ExecuteDelete();
        await _dbContext.SaveChangesAsync();

        Container container = await GetContainer();
        await container.DeleteContainerAsync();

        await _cacheService.RemoveAsync(OPERATIONATTRIBUTE_KEY);
        await _cacheService.RemoveAsync(OPERATION_KEY);
        await _cacheService.RemoveAsync(OPERATIONWITHATTRIBUTE_KEY);

        await Task.CompletedTask;
    }

    public async Task AddAttributes(object attributes)
    {
        Container container = await GetContainer();
        var item = JsonConvert.DeserializeObject<dynamic>(attributes.ToString());
        var idOperation = (int?)((dynamic)item).id ?? throw new NotFoundException("Operation not found");
        var codeOperation = (string?)((dynamic)item).code ?? throw new NotFoundException("Operation not found");

        if (!_dbContext.Operations.Any(x => x.Id == idOperation))
            throw new NotFoundException("Operation not found");

        var response = await container.ReadItemStreamAsync(idOperation.ToString(), new PartitionKey(codeOperation));

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            await container.CreateItemAsync(item);
        else
            await container.UpsertItemAsync(item);

        await _cacheService.RemoveAsync(OPERATIONATTRIBUTE_KEY);
        await _cacheService.RemoveAsync(OPERATIONWITHATTRIBUTE_KEY);
    }

    public async Task<IEnumerable<object>> GetAllAttribute()
    {
        var cacheData = await _cacheService.GetAsync<IEnumerable<object>>(OPERATIONATTRIBUTE_KEY);
        if (cacheData != null && cacheData.Count() > 0)
            return cacheData;

        Container container = await GetContainer();
        using FeedIterator<object> setIterator = container
            .GetItemLinqQueryable<object>()
            .ToFeedIterator<object>();

        var result = new List<object>();

        while (setIterator.HasMoreResults)
        {
            foreach (var item in await setIterator.ReadNextAsync())
            {
                var stringJson = JsonConvert.SerializeObject(item);
                var obj = JsonConvert.DeserializeObject<ExpandoObject>(stringJson);
                result.Add(obj);
            }
        }

        var expiryTime = DateTimeOffset.Now.AddMinutes(30);
        await _cacheService.SetAsync(OPERATIONATTRIBUTE_KEY, result, expiryTime);
        return result;
    }


    private async Task<Container> GetContainer()
    {
        var db = await _cosmosClient.CreateDatabaseIfNotExistsAsync("OperationDB");
        var container = await db.Database.CreateContainerIfNotExistsAsync($"OperationAttributes", $"/code", 1000);
        if (container.StatusCode == System.Net.HttpStatusCode.OK || container.StatusCode == System.Net.HttpStatusCode.Created)
            return container.Container;
        else
            throw new CreateResourceException("container not created");
    }

}
