using Microsoft.Azure.Cosmos;
using System.Dynamic;

namespace Operation.Application.Contracts.Services;

public interface ICosmosService
{
    Task<ExpandoObject> Get(string containerName, string id, PartitionKey partitionKey, CancellationToken cancellationToken = default);
    Task<List<ExpandoObject>> GetAll(string containerName, CancellationToken cancellationToken = default);
    Task AddOrEdit(string containerName, string id, PartitionKey partitionKey, dynamic item, CancellationToken cancellationToken = default);
    Task Delete(string containerName, string id, PartitionKey partitionKey, CancellationToken cancellationToken = default);
    Task DeleteAll(string containerName, CancellationToken cancellationToken = default);
}
