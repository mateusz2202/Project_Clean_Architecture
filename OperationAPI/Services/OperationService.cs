using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OperationAPI.Data;
using OperationAPI.Entities;
using OperationAPI.Exceptions;
using OperationAPI.Interfaces;
using OperationAPI.Models;
using RabbitMQ.Client;
using System.Dynamic;
using System.Net;
using System.Text;

namespace OperationAPI.Services;

public class OperationService : IOperationService
{
    private const string OPERATION_KEY = "operation";
    private const string OPERATIONATTRIBUTE_KEY = "operationAttribute";
    private const string OPERATIONWITHATTRIBUTE_KEY = "operationWithAttribute";
    private const string EXCHANGE_OPERATION = "EXCHANGE_OPERATION";

    private readonly OperationDbContext _dbContext;
    private readonly CosmosClient _cosmosClient;
    private readonly ICacheService _cacheService;
    private readonly IRabbitMqService _rabbitMqService;

    public OperationService(
        OperationDbContext dbContext,
        CosmosClient cosmosClient,
        ICacheService cacheService,
        IRabbitMqService rabbitMqService)
    {
        _dbContext = dbContext;
        _cosmosClient = cosmosClient;
        _cacheService = cacheService;
        _rabbitMqService = rabbitMqService;
    }

    public async Task<IEnumerable<Operation>> GetAllOperations()
    {
        var cacheData = await _cacheService.GetAsync<IEnumerable<Operation>>(OPERATION_KEY);

        if (cacheData != null && cacheData.Any())
            return cacheData;

        var result = await _dbContext.Operations.ToListAsync();

        await _cacheService.SetAsync(OPERATION_KEY, result, DateTimeOffset.Now.AddMinutes(30));

        return result;
    }

    public async Task<IEnumerable<dynamic>> GetAllAttributes()
    {
        var cacheData = await _cacheService.GetAsync<IEnumerable<ExpandoObject>>(OPERATIONATTRIBUTE_KEY);
        if (cacheData != null && cacheData.Any())
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

        await _cacheService.SetAsync(OPERATIONATTRIBUTE_KEY, result, DateTimeOffset.Now.AddMinutes(30));

        return result;
    }

    public async Task<IEnumerable<dynamic>> GetAllOperationsWithAtrributes()
    {
        var cacheData = await _cacheService.GetAsync<IEnumerable<dynamic>>(OPERATIONWITHATTRIBUTE_KEY);
        if (cacheData != null && cacheData.Any())
            return cacheData;

        var operations = await GetAllOperations();
        var attributes = await GetAllAttributes();

        var union = from x in operations
                    join z in attributes on x.Id.ToString() equals z.id into gj
                    from subZ in gj.DefaultIfEmpty()
                    select new { Operation = x, Atrributes = JsonConvert.DeserializeObject<ExpandoObject>(JsonConvert.SerializeObject(subZ)) };

        var result = union.ToList();

        await _cacheService.SetAsync(OPERATIONWITHATTRIBUTE_KEY, result, DateTimeOffset.Now.AddMinutes(30));

        return result;
    }

    public async Task<Operation> GetOperationById(int id)
    {
        var operation = await _dbContext.Operations.FirstOrDefaultAsync(x => x.Id == id)
                         ?? throw new NotFoundException("Operation not found");
        return operation;
    }

    public async Task<Operation> GetOperationByCode(string code)
    {
        var operation = await _dbContext.Operations.FirstOrDefaultAsync(x => x.Code.ToUpper() == code.ToLower())
                         ?? throw new NotFoundException("Operation not found");
        return operation;
    }

    public async Task<dynamic> GetAttributeById(int id)
    {
        var operation = await GetOperationById(id);
        var attribute = await GetAttribute(operation.Id.ToString(), new PartitionKey(operation.Code));

        return attribute;
    }

    public async Task<dynamic> GetAttributeByCoded(string code)
    {
        var operation = await GetOperationByCode(code);
        var attribute = await GetAttribute(operation.Id.ToString(), new PartitionKey(operation.Code));
        return attribute;
    }

    public async Task<dynamic> GetOperationWithAttributeById(int id)
    {
        var operation = await GetOperationById(id);
        var attribute = await GetAttributeById(id);

        return new { Operation = operation, Attribute = attribute };
    }

    public async Task<dynamic> GetOperationWithAttributeByCoded(string code)
    {
        var operation = await GetOperationByCode(code);
        var attribute = await GetAttributeByCoded(code);

        return new { Operation = operation, Attribute = attribute };
    }

    public async Task<Operation> AddOperation(CreateOperationDTO dto)
    {
        var operation = await _dbContext.Operations.AddAsync(new Entities.Operation() { Name = dto.Name, Code = dto.Code });
        await _dbContext.SaveChangesAsync();

        await ClearCache();     

        return operation.Entity;
    }

    public async Task AddAttributes(object attributes)
    {
        Container container = await GetContainer();
        var item = JsonConvert.DeserializeObject<dynamic>(attributes.ToString());
        var idOperation = (int?)((dynamic)item).id ?? throw new NotFoundException("Operation not found");
        var codeOperation = (string?)((dynamic)item).code ?? throw new NotFoundException("Operation not found");

        if (!_dbContext.Operations.Any(x => x.Id == idOperation && x.Code == codeOperation))
            throw new NotFoundException("Operation not found");

        var response = await container.ReadItemStreamAsync(idOperation.ToString(), new PartitionKey(codeOperation));

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            await container.CreateItemAsync(item);
        else
            await container.UpsertItemAsync(item);

        await ClearCache();     

        await Task.CompletedTask;
    }

    public async Task AddOperationWithAttributes(CreateOperationWithAttributeDTO dto)
    {
        var addObj = await AddOperation(dto.CreateOperationDTO);
        var item = JsonConvert.DeserializeObject<dynamic>(dto?.Attributes?.ToString());
        ((dynamic)item).id = addObj.Id.ToString();
        ((dynamic)item).code = addObj.Code;

        string json = JsonConvert.SerializeObject(item);
        await AddAttributes(json);

        await ClearCache();
  
        await Task.CompletedTask;
    }

    public async Task DeleteOperationById(int id)
    {
        var operation = await GetOperationById(id);

        await DeleteAttribute(operation.Id.ToString(), new PartitionKey(operation.Code));

        _dbContext.Operations.Remove(operation);
        await _dbContext.SaveChangesAsync();

        await ClearCache();      

        await Task.CompletedTask;
    }

    public async Task DeleteOperationByCode(string code)
    {
        var operation = await GetOperationByCode(code);

        await DeleteAttribute(operation.Id.ToString(), new PartitionKey(operation.Code));

        _dbContext.Operations.Remove(operation);
        await _dbContext.SaveChangesAsync();

        await ClearCache();
      
        await Task.CompletedTask;
    }

    public async Task DeleteAttributeById(int id)
    {
        var operation = await GetOperationById(id);

        await DeleteAttribute(operation.Id.ToString(), new PartitionKey(operation.Code));

        await ClearCache();       

        await Task.CompletedTask;
    }

    public async Task DeleteAttributeByCode(string code)
    {
        var operation = await GetOperationByCode(code);

        await DeleteAttribute(operation.Id.ToString(), new PartitionKey(operation.Code));

        await ClearCache(); 

        await Task.CompletedTask;
    }

    public async Task DeleteAll()
    {
        _dbContext.Operations.ExecuteDelete();
        await _dbContext.SaveChangesAsync();

        Container container = await GetContainer();
        await container.DeleteContainerAsync();

        await ClearCache();      

        await Task.CompletedTask;
    }

    public async Task ClearCache()
    {
        await _cacheService.RemoveAsync(OPERATIONATTRIBUTE_KEY);
        await _cacheService.RemoveAsync(OPERATION_KEY);
        await _cacheService.RemoveAsync(OPERATIONWITHATTRIBUTE_KEY);
        SendInfoAddedOperation();
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

    private async Task<dynamic> GetAttribute(string id, PartitionKey partitionKey)
    {
        var container = await GetContainer();
        var item = await container.ReadItemStreamAsync(id, partitionKey);
        if (item != null && item.StatusCode == HttpStatusCode.OK)
        {
            using StreamReader streamReader = new(item.Content);
            string content = await streamReader.ReadToEndAsync();
            var result = JsonConvert.DeserializeObject<ExpandoObject>(content);
            return result;
        }
        else
            return new { };
    }

    private async Task DeleteAttribute(string id, PartitionKey partitionKey)
    {
        var container = await GetContainer();
        var item = await container.ReadItemStreamAsync(id, partitionKey);

        if (item != null && item.StatusCode == HttpStatusCode.OK)
            await container.DeleteItemAsync<object>(id, partitionKey);

        await Task.CompletedTask;
    }

    private void SendInfoAddedOperation()
    {
        using var connection = _rabbitMqService.CreateChannel();
        using var channel = connection.CreateModel();     

        channel.ExchangeDeclare(exchange: EXCHANGE_OPERATION, type: ExchangeType.Fanout); 

        var body = Encoding.UTF8.GetBytes("refresh_operation");

        channel.BasicPublish(exchange: EXCHANGE_OPERATION,
                        routingKey: string.Empty,
                        basicProperties: null,
                        body: body);
    }
}
