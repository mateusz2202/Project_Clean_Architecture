namespace OperationAPI.Interfaces;

public interface IOperationAttributeService
{
    public Task AddAttributes(object attributes);
    public Task<IEnumerable<object>> GetAll();
}
