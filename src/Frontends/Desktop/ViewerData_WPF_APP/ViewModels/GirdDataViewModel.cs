﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewerData_WPF_APP.Interfaces;
using ViewerData_WPF_APP.Models;


namespace ViewerData_WPF_APP.ViewModels;

public partial class GirdDataViewModel : ObservableObject
{
    private const string EXCHANGE_OPERATION = "EXCHANGE_OPERATION";

    private readonly IOperationServices _operationServices;    
    private readonly IModel _channel;
    public GirdDataViewModel(IOperationServices operationServices, IRabbitMqService rabbitMqService)
    {
        LoadedCommand = new AsyncRelayCommand(Loaded);
        UnloadedCommand = new AsyncRelayCommand(Unloaded);
        _operationServices = operationServices; 
        _channel = rabbitMqService.CreateChannel().CreateModel();
    }

    public ICommand LoadedCommand { get; set; }
    public ICommand UnloadedCommand { get; set; }

    [ObservableProperty]
    private ObservableCollection<Operation>? gridData; 

    private async Task Loaded()
    {
        await SubscribeQueue();
        await LoadData();
        await Task.CompletedTask;
    }

    private async Task SubscribeQueue()
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

        await Task.CompletedTask;
    }

    private async Task DoJobFromQueue(object sender, BasicDeliverEventArgs @event)
    {      
        var body = @event.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        if (!string.IsNullOrEmpty(message) && message == "refresh_operation")
            await LoadData();       
    }
 

    private async Task Unloaded()
    {
        if (_channel != null && _channel.IsOpen)
            _channel.Close();

        await Task.CompletedTask;
    }

    private async Task LoadData()
    {
        GridData = await _operationServices.GetOperations();
    }


}
