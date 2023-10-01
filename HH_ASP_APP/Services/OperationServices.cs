using HH_ASP_APP.Interfaces;
using HH_ASP_APP.Models;
using Flurl.Http;
using Newtonsoft.Json;
using System.Text;

namespace HH_ASP_APP.Services;


public class OperationServices : IOperationServices
{
    private const string HOSTNAME = "localhost";
    private const string PORT = "5161";
    private const string BASE_URL = $"http://{HOSTNAME}:{PORT}";

    public async Task<IEnumerable<Operation>> GetOperations()
       => await $"{BASE_URL}/Operation/operations"
                .GetJsonAsync<IEnumerable<Operation>>();

    public async Task<Operation> GetOperationById(int operationId)
        => await $"{BASE_URL}/Operation/operations/id/{operationId}"
                 .GetJsonAsync<Operation>();

    public async Task DeleteOperationById(int operationId)
        => await $"{BASE_URL}/Operation/id/{operationId}"
                 .DeleteAsync();

    public async Task CreateOperation(Operation operation)
        => await $"{BASE_URL}/Operation/operations"
                 .PostAsync(new StringContent(JsonConvert.SerializeObject(new { operation.Code, operation.Name }), Encoding.UTF8, "application/json"));

   

}
