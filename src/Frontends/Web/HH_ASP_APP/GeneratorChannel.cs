using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using HH_ASP_APP.Interfaces;
using System.Text;

namespace HH_ASP_APP;

public class GeneratorChannel
{
    private const string EXCHANGE_OPERATION = "EXCHANGE_OPERATION";
  
    private readonly IModel _channel;
    public GeneratorChannel(IRabbitMqService rabbitMqService)
    {        
        _channel = rabbitMqService.CreateChannel().CreateModel();
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
            
        }
        await Task.CompletedTask;
    }
}
