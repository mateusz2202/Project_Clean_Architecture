using HH_ASP_APP.Models;

namespace HH_ASP_APP.Interfaces;

public interface IOperationServices
{
    public Task<IEnumerable<Operation>> GetOperations();
    public Task<Operation> GetOperationById(int operationId);
    public Task DeleteOperationById(int operationId);
    public Task CreateOperation(Operation operation);
}
