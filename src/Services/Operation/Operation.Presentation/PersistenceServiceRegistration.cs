using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Operation.Application.Contracts.Repositories;
using Operation.Persistence.Repositories;

namespace Operation.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    => services
            .AddDbContext<OperationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbConnectionString"),
                b => b.MigrationsAssembly(typeof(OperationDbContext).Assembly.FullName)))
            .AddRepositories();


    private static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
            .AddTransient<IOperationRepository, OperationRepository>()
            .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

}

