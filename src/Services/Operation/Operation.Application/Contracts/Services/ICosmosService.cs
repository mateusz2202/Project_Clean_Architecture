using Microsoft.Azure.Cosmos;

namespace Operation.Application.Contracts.Services;

public interface ICosmosService
{
    Task<Container> GetContainerOperation();
}
