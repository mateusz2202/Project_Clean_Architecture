using CommunityToolkit.Mvvm.DependencyInjection;
using ViewerData_MAUI_APP.ViewModels;

namespace ViewerData_MAUI_APP.Views;

public partial class GridDataView : ContentPage
{
    public GridDataView()
    {
        InitializeComponent();
        BindingContext = Ioc.Default.GetRequiredService<GirdDataViewModel>();
    }
}