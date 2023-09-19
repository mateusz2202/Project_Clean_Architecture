namespace OperationAPI.Interfaces;

public interface IOperationService
{
    public Task AddOperation(string name, string code);
}
