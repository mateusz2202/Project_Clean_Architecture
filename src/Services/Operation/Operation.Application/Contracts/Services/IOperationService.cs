using Microsoft.Azure.Cosmos;

namespace Operation.Application.Contracts.Services;

public interface IOperationService
{
    Task<dynamic> GetAttribute(string id, PartitionKey partitionKey);
    Task AddOrEditAtributes(string id, PartitionKey partitionKey, dynamic item, CancellationToken cancellationToken = default);
    Task DeleteAttribute(string id, PartitionKey partitionKey, CancellationToken cancellationToken = default);
    Task DeleteAllAtribue();
    void SendInfoAddedOperation();
}
