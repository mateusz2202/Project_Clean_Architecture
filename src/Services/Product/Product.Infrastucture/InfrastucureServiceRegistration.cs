using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Interfaces.Services;
using Product.Infrastucture.Services;

namespace Product.Infrastucture;

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