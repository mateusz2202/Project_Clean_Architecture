using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Interfaces.Repositories;
using Product.Persistence.Repositories;


namespace Product.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    => services
            .AddDbContext<ProductDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbConnectionString")))
            .AddRepositories();


    private static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
            .AddTransient<IProductRepository, ProductRepository>()
            .AddTransient<IBrandRepository, BrandRepository>()
            .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
          


}

