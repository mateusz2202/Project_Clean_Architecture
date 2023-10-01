using HH_ASP_APP.Interfaces;
using HH_ASP_APP.Models;
using Flurl.Http;

namespace HH_ASP_APP.Services;

public class OperationServices : IOperationServices
{
    private const string HOSTNAME = "localhost";
    private const string PORT = "5161";
    private const string BASE_URL = $"http://{HOSTNAME}:{PORT}";

    public async Task<IEnumerable<Operation>> GetOperations()
       => await $"{BASE_URL}/Operation/operations".GetJsonAsync<IEnumerable<Operation>>();

    public async Task<Operation> GetOperationById(int operationId)
        => await $"{BASE_URL}/Operation/operations/id/{operationId}".GetJsonAsync<Operation>();

    public async Task UpdateOperationById(int operationId, Operation operation)
        => await $"{BASE_URL}/Operation/id/{operationId}".PutJsonAsync(new { createOperationDTO = operation, attributes = new { id = "", code = "" } });


    public async Task DeleteOperationById(int operationId)
        => await $"{BASE_URL}/Operation/id/{operationId}".DeleteAsync();

    public async Task CreateOperation(Operation operation)
        => await $"{BASE_URL}/Operation/operations".PostJsonAsync(operation);

}
