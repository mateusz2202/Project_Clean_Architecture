using OperationAPI.Entities;
using OperationAPI.Models;

namespace OperationAPI.Interfaces;

public interface IOperationService
{
    public Task<IEnumerable<Operation>> GetAllOperations();
    public Task<IEnumerable<dynamic>> GetAllAttributes();
    public Task<IEnumerable<dynamic>> GetAllOperationsWithAtrributes();
    public Task<Operation> GetOperationById(int id);
    public Task<Operation> GetOperationByCode(string code);
    public Task<dynamic> GetAttributeById(int id);
    public Task<dynamic> GetAttributeByCoded(string code);
    public Task<dynamic> GetOperationWithAttributeById(int id);
    public Task<dynamic> GetOperationWithAttributeByCoded(string code);
    public Task<Operation> AddOperation(CreateOperationDTO dto);
    public Task AddAttributes(object attributes);
    public Task AddOperationWithAttributes(CreateOperationWithAttributeDTO dto);
    public Task UpdateOperationWithAttributes(int id, CreateOperationWithAttributeDTO dto);
    public Task DeleteOperationById(int id);
    public Task DeleteOperationByCode(string code);
    public Task DeleteAttributeById(int id);
    public Task DeleteAttributeByCode(string code);
    public Task DeleteAll();
    public Task ClearCache();

}
