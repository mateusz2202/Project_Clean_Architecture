using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json;
using OperationAPI.Data;
using OperationAPI.Exceptions;
using OperationAPI.Interfaces;
using System.Dynamic;

namespace OperationAPI.Services;

public class OperationAttributeService : IOperationAttributeService
{
    private readonly CosmosClient _cosmosClient;
    private readonly OperationDbContext _dbContext;
    public OperationAttributeService(CosmosClient cosmosClient, OperationDbContext dbContext)
    {
        _cosmosClient = cosmosClient;
        _dbContext = dbContext;
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
    }

    public async Task<IEnumerable<object>> GetAll()
    {
        Container container = await GetContainer();
        using FeedIterator<object> setIterator = container
            .GetItemLinqQueryable<object>()
            .ToFeedIterator<object>();

        var result = new List<object>();
       
        while (setIterator.HasMoreResults)
        {
            foreach (ExpandoObject item in await setIterator.ReadNextAsync())
            {
                var stringJson=JsonConvert.SerializeObject(item);
                var xd = JsonConvert.DeserializeObject<ExpandoObject>(stringJson);
                result.Add(item);
            }
        }
        return result;
    }

    private async Task<Container> GetContainer()
    {
        var db = await _cosmosClient.CreateDatabaseIfNotExistsAsync("OperationDB");
        var container = await db.Database.CreateContainerIfNotExistsAsync($"OperationAttributes", $"/code", 1000);
        if (container.StatusCode != System.Net.HttpStatusCode.OK) throw new CreateResourceException("container not created");
        return container.Container;
    }
}
