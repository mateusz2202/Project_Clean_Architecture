using OperationAPI.Entities;

namespace OperationAPI.Interfaces;

public interface IOperationService
{
    public Task<IEnumerable<Operation>> GetAll();
    public Task AddOperation(string name, string code);
}
