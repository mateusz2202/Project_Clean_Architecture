using RabbitMQ.Client;

namespace ViewerData_WPF_APP.Interfaces;

public interface IRabbitMqService
{
    IConnection CreateChannel();
}
