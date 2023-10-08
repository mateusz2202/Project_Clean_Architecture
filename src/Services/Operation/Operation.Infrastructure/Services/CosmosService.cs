using Microsoft.Azure.Cosmos;
using Operation.Application.Contracts.Services;

namespace Operation.Infrastructure.Services;

public class CosmosService : ICosmosService
{
    private readonly CosmosClient _client;
    public CosmosService(CosmosClient client)
    {
        _client = client;
    }
    public async Task<Container> GetContainerOperation()
    {
        var db = await _client.CreateDatabaseIfNotExistsAsync("OperationDB");
        var container = await db.Database.CreateContainerIfNotExistsAsync($"OperationAttributes", $"/code", 1000);
        if (container.StatusCode == System.Net.HttpStatusCode.OK || container.StatusCode == System.Net.HttpStatusCode.Created)
            return container.Container;
        else
            throw new Exception("container not created");
    }
}
