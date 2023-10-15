using Document.Application.Interfaces.Services;
using Document.Infrastucture.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Document.Infrastucture;

public static class InfrastucureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services      
            .AddServices();

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddTransient<IExcelService, ExcelService>()
            .AddTransient<IUploadService, UploadService>();
          
}
