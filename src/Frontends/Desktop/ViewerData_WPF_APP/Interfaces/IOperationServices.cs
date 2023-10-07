using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ViewerData_WPF_APP.Models;

namespace ViewerData_WPF_APP.Interfaces;

public interface IOperationServices
{
    public Task<ObservableCollection<Operation>> GetOperations();
}
