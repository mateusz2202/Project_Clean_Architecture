using Microsoft.Azure.Cosmos;

namespace Operation.Application.Contracts.Services;

public interface IOperationService
{
    void SendInfoAddedOperation();
}
