using CommunityToolkit.Mvvm.DependencyInjection;
using System.Windows;
using ViewerData_WPF_APP.ViewModels;

namespace ViewerData_WPF_APP.Views
{
    /// <summary>
    /// Interaction logic for GirdDataView.xaml
    /// </summary>
    public partial class GirdDataView : Window
    {
        public GirdDataView()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<GirdDataViewModel>();
        }
    }
}
