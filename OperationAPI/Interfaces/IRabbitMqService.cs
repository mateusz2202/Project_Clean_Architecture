using RabbitMQ.Client;

namespace OperationAPI.Interfaces;

public interface IRabbitMqService
{
    IConnection CreateChannel();
}
