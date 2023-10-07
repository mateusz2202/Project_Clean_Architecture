using RabbitMQ.Client;

namespace ViewerData_MAUI_APP.Interfaces;

public interface IRabbitMqService
{
    IConnection CreateChannel();
}
