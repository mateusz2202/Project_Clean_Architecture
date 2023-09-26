using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewerData_WPF_APP.Interfaces;
using ViewerData_WPF_APP.Models;

namespace ViewerData_WPF_APP.ViewModels;

public partial class GirdDataViewModel : ObservableObject
{
    private readonly IOperationServices _operationServices;
    public GirdDataViewModel(IOperationServices operationServices)
    {
        LoadedCommand = new AsyncRelayCommand(Loaded);
        UnloadedCommand = new AsyncRelayCommand(Unloaded);
        _operationServices = operationServices;
    }

    public ICommand LoadedCommand { get; set; }
    public ICommand UnloadedCommand { get; set; }

    [ObservableProperty]
    private ObservableCollection<Operation> gridData;


    private async Task Loaded()
    {
        GridData = await _operationServices.GetOperations();
        await Task.CompletedTask;
    }

    private async Task Unloaded()
    {
        await Task.CompletedTask;
    }


}
