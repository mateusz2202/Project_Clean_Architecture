using Flurl.Http;
using System.Collections.ObjectModel;
using ViewerData_MAUI_APP.Interfaces;
using ViewerData_MAUI_APP.Models;

namespace ViewerData_MAUI_APP.Services;

public class OperationServices : IOperationServices
{
    private const string HOSTNAME = "localhost";
    private const string PORT = "5161";
    private const string BASE_URL = $"http://{HOSTNAME}:{PORT}";
    public async Task<ObservableCollection<Operation>> GetOperations()
    {
        var operations = await $"{BASE_URL}/Operation/operations".GetJsonAsync<IEnumerable<Operation>>();
        return operations.ToObservableCollection();
    }
}