using CommunityToolkit.Mvvm.DependencyInjection;
using ViewerData_MAUI_APP.Interfaces;
using ViewerData_MAUI_APP.Services;
using ViewerData_MAUI_APP.ViewModels;

namespace ViewerData_MAUI_APP
{
    public partial class App : Application
    {
        public App()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<GirdDataViewModel>();
            serviceCollection.Configure<RabbitMqConfiguration>(conf =>
            {
                conf.HostName = "localhost";
                conf.Username = "guest";
                conf.Password = "guest";
            });
            serviceCollection.AddScoped<IRabbitMqService, RabbitMqService>();
            serviceCollection.AddScoped<IOperationServices, OperationServices>();

            Ioc.Default.ConfigureServices(serviceCollection.BuildServiceProvider());
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}