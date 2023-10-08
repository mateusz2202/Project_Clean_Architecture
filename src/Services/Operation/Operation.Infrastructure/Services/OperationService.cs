using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using Operation.Application.Contracts.Services;
using Operation.Shared.Constans;
using RabbitMQ.Client;
using System.Dynamic;
using System.Net;
using System.Text;

namespace Operation.Infrastructure.Services;

public class OperationService : IOperationService
{
    private readonly ICosmosService _cosmosService;
    private readonly IRabbitMqService _rabbitMqService;

    public OperationService(ICosmosService cosmosService, IRabbitMqService rabbitMqService)
    {
        _cosmosService = cosmosService;
        _rabbitMqService = rabbitMqService;
    }

    public async Task<dynamic> GetAttribute(string id, PartitionKey partitionKey)
    {
        var container = await _cosmosService.GetContainerOperation();
        var item = await container.ReadItemStreamAsync(id, partitionKey);
        if (item != null && item.StatusCode == HttpStatusCode.OK)
        {
            using StreamReader streamReader = new(item.Content);
            string content = await streamReader.ReadToEndAsync();
            var result = JsonConvert.DeserializeObject<ExpandoObject>(content);
            return result;
        }
        else
            return new { };
    }

    public async Task AddOrEditAtributes(string id, PartitionKey partitionKey, dynamic item, CancellationToken cancellationToken = default)
    {
        Container container = await _cosmosService.GetContainerOperation();
        var response = await container.ReadItemStreamAsync(id, partitionKey, cancellationToken: cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            await container.CreateItemAsync(item);
        else
            await container.UpsertItemAsync(item);
    }

    public async Task DeleteAttribute(string id, PartitionKey partitionKey, CancellationToken cancellationToken = default)
    {
        var container = await _cosmosService.GetContainerOperation();
        var item = await container.ReadItemStreamAsync(id, partitionKey, cancellationToken: cancellationToken);

        if (item != null && item.StatusCode == HttpStatusCode.OK)
            await container.DeleteItemAsync<object>(id, partitionKey, cancellationToken: cancellationToken);

        await Task.CompletedTask;
    }
    public async Task DeleteAllAtribue()
    {
        var container = await _cosmosService.GetContainerOperation();
        await container.DeleteContainerAsync();
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
