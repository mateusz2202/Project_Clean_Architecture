using Microsoft.Extensions.DependencyInjection;
using Operation.Application.Contracts.Services;
using Operation.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using StackExchange.Redis;
using Operation.Infrastructure.Configuration;
namespace Operation.Infrastructure;

public static class InfrastucureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton<CosmosClient>((s) => new CosmosClient(configuration.GetConnectionString("CosmosDbConnectionString")))
            .AddSingleton<IDatabase>(cfg =>
                        {
                            var redisConnection =
                                ConnectionMultiplexer
                                    .Connect(configuration.GetConnectionString("RedisConnectionString") ?? string.Empty);
                        
                            return redisConnection.GetDatabase();
                        })
            .Configure<RabbitMqConfiguration>(conf => configuration.GetSection(nameof(RabbitMqConfiguration)).Bind(conf))
            .AddTransient<ICacheService, CacheService>()
            .AddTransient<ICosmosService, CosmosService>()
            .AddTransient<IOperationService, OperationService>()
            .AddTransient<IRabbitMqService, RabbitMqService>();
    

}
