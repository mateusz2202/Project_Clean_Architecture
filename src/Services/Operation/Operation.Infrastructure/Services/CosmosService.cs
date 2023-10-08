using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Operation.Application.Common.Exceptions;
using Operation.Application.Contracts.Services;
using Operation.Infrastructure.Configuration;
using System.Dynamic;
using System.Net;

namespace Operation.Infrastructure.Services;

public class CosmosService : ICosmosService
{
    private readonly CosmosClient _client;
    private readonly CosmosDbConfiguration _cosmosDbConfiguration;
    public CosmosService(IOptions<CosmosDbConfiguration> cosmosDbConfiguration, CosmosClient client)
    {
        _cosmosDbConfiguration = cosmosDbConfiguration.Value;
        _client = client;
    }

    public async Task<ExpandoObject> Get(string containerName, string id, PartitionKey partitionKey, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(containerName, cancellationToken);

        var item = await container.ReadItemStreamAsync(id, partitionKey, cancellationToken: cancellationToken);

        if (item != null && item.StatusCode == HttpStatusCode.OK)
        {
            using StreamReader streamReader = new(item.Content);
            string content = await streamReader.ReadToEndAsync(cancellationToken);
            var result = JsonConvert.DeserializeObject<ExpandoObject>(content);
            return result;
        }
        else
            return new ExpandoObject();
    }

    public async Task<List<ExpandoObject>> GetAll(string containerName, CancellationToken cancellationToken = default)
    {
        Container container = await GetContainer(name: containerName, cancellationToken: cancellationToken);

        using FeedIterator<object> setIterator = container
            .GetItemLinqQueryable<object>()
            .ToFeedIterator<object>();

        var result = new List<ExpandoObject>();

        while (setIterator.HasMoreResults)
        {
            foreach (var item in await setIterator.ReadNextAsync(cancellationToken))
            {
                var stringJson = JsonConvert.SerializeObject(item);
                var obj = JsonConvert.DeserializeObject<ExpandoObject>(stringJson);
                result.Add(obj);
            }
        }
        return result;
    }

    public async Task AddOrEdit(string containerName, string id, PartitionKey partitionKey, dynamic item, CancellationToken cancellationToken = default)
    {
        Container container = await GetContainer(containerName, cancellationToken);

        var response = await container.ReadItemStreamAsync(id, partitionKey, cancellationToken: cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
            await container.CreateItemAsync(item);
        else
            await container.UpsertItemAsync(item);
    }

    public async Task Delete(string containerName, string id, PartitionKey partitionKey, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(containerName, cancellationToken);

        var item = await container.ReadItemStreamAsync(id, partitionKey, cancellationToken: cancellationToken);

        if (item != null && item.StatusCode == HttpStatusCode.OK)
            await container.DeleteItemAsync<object>(id, partitionKey, cancellationToken: cancellationToken);

        await Task.CompletedTask;
    }

    public async Task DeleteAll(string containerName, CancellationToken cancellationToken = default)
    {
        var container = await GetContainer(containerName, cancellationToken);
        await container.DeleteContainerAsync(cancellationToken: cancellationToken);
    }

    private async Task<Container> GetContainer(string name, CancellationToken cancellationToken = default)
    {
        var conf = _cosmosDbConfiguration
            .Containers
            .FirstOrDefault(x => x.Name == name)
            ?? throw new NotFoundException($"not found configuration container {name}");

        var db = await _client
            .CreateDatabaseIfNotExistsAsync(id: _cosmosDbConfiguration.DBName,
                                            cancellationToken: cancellationToken);

        var containerResponse = await db
            .Database
            .CreateContainerIfNotExistsAsync(containerProperties: new ContainerProperties(conf.Id, conf.PartitionKeyPath),
                                             throughput: conf.Throughput,
                                             cancellationToken: cancellationToken);

        if (containerResponse.StatusCode is not System.Net.HttpStatusCode.OK and System.Net.HttpStatusCode.Created)
            throw new BadRequestException("container not created");

        return containerResponse.Container;
    }
}
