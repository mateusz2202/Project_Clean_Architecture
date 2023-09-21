using OperationAPI.Entities;
using OperationAPI.Models;

namespace OperationAPI.Interfaces;

public interface IOperationService
{
    public Task<IEnumerable<Operation>> GetAll();
    public Task AddOperation(CreateOperationDTO dto);
    public Task DeleteOperation(string code);
    public Task DeleteAll();
    public Task AddAttributes(object attributes);
    public Task<IEnumerable<object>> GetAllAttribute();
}
