using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using RabbitMQ.Client;
using System.Text;

namespace Operation.Infrastructure.Services;

public class OperationService : IOperationService
{ 
    private readonly IRabbitMqService _rabbitMqService;

    public OperationService(IRabbitMqService rabbitMqService)
    {       
        _rabbitMqService = rabbitMqService;
    } 

    public void SendInfoAddedOperation()
    {
        using var connection = _rabbitMqService.CreateChannel();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(exchange: ApplicationConstants.RabbitMq.EXCHANGE_OPERATION, type: ExchangeType.Fanout);

        var body = Encoding.UTF8.GetBytes("refresh_operation");

        channel.BasicPublish(exchange: ApplicationConstants.RabbitMq.EXCHANGE_OPERATION,
                        routingKey: string.Empty,
                        basicProperties: null,
                        body: body);
    }
   
}
