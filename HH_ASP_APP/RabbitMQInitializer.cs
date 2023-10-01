using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using HH_ASP_APP.Interfaces;
using Microsoft.AspNetCore.SignalR;
using HH_ASP_APP.Hubs;

namespace HH_ASP_APP;

public class RabbitMQInitializer 
{
    private const string EXCHANGE_OPERATION = "EXCHANGE_OPERATION";

    protected readonly IServiceProvider _serviceProvider;
    private readonly IModel _channel;
    public RabbitMQInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _channel = _serviceProvider
                   .GetRequiredService<IRabbitMqService>()
                   .CreateChannel()
                   .CreateModel();
    }

    public void Initialize()
    {
        SubscribeQueue();
    }

    private void SubscribeQueue()
    {
        _channel.ExchangeDeclare(exchange: EXCHANGE_OPERATION, type: ExchangeType.Fanout);

        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName,
        exchange: EXCHANGE_OPERATION,
                          routingKey: string.Empty);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += DoJobFromQueue;

        _channel.BasicConsume(queue: queueName,
                     autoAck: true,
                     consumer: consumer);
    }

    private async Task DoJobFromQueue(object sender, BasicDeliverEventArgs @event)
    {
        var body = @event.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        if (!string.IsNullOrEmpty(message) && message == "refresh_operation")
        {
            var rabbitMQHub = (IHubContext<RabbitMQHub>)_serviceProvider.GetRequiredService(typeof(IHubContext<RabbitMQHub>));
            await rabbitMQHub.Clients.All.SendAsync(message, "test");
        }
        await Task.CompletedTask;
    }
   
}
