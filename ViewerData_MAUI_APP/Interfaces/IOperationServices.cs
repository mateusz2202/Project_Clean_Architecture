using System.Collections.ObjectModel;
using ViewerData_MAUI_APP.Models;

namespace ViewerData_MAUI_APP.Interfaces;

public interface IOperationServices
{
    public Task<ObservableCollection<Operation>> GetOperations();
}
