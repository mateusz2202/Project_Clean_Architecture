using Document.Application.Interfaces.Repositories;
using Document.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Document.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    => services
            .AddDbContext<DocumentDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbConnectionString")))
            .AddRepositories();       


    private static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .AddTransient(typeof(IRepositoryAsync<,>), typeof(RepositoryAsync<,>))
            .AddTransient<IDocumentRepository, DocumentRepository>()
            .AddTransient<IDocumentTypeRepository, DocumentTypeRepository>()
            .AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
            .AddTransient(typeof(IExtendedAttributeUnitOfWork<,,>), typeof(ExtendedAttributeUnitOfWork<,,>));


}
