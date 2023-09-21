using OperationAPI.Entities;
using OperationAPI.Models;

namespace OperationAPI.Interfaces;

public interface IOperationService
{
    public Task<IEnumerable<Operation>> GetAll();
    public Task<IEnumerable<dynamic>> GetAllWithAtrributes();
    public Task<Operation> Get(int id);
    public Task<Operation> AddOperation(CreateOperationDTO dto);
    public Task AddOperationWithAttributes(CreateOperationWithAttributeDTO dto);
    public Task DeleteOperation(string code);
    public Task DeleteAll();
    public Task AddAttributes(object attributes);
    public Task<IEnumerable<object>> GetAllAttribute();
}
