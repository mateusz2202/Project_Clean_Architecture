using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ViewerData_WPF_APP.Interfaces;
using ViewerData_WPF_APP.Services;
using ViewerData_WPF_APP.ViewModels;
using ViewerData_WPF_APP.Views;

namespace ViewerData_WPF_APP
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<GirdDataViewModel>();
            serviceCollection.AddScoped<IOperationServices, OperationServices>();

            Ioc.Default.ConfigureServices(serviceCollection.BuildServiceProvider());
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            GirdDataView view = new();
            view.Show();
        }
    }
}
