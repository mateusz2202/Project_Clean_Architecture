

using RabbitMQ.Client;

namespace HH_ASP_APP.Interfaces;

public interface IRabbitMqService
{
    IConnection CreateChannel();
}

