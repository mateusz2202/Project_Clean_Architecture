using RabbitMQ.Client;

namespace Operation.Application.Contracts.Services;

public interface IRabbitMqService
{
    IConnection CreateChannel();
}
