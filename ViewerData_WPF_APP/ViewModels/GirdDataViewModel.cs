using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Input;
using ViewerData_WPF_APP.Interfaces;
using ViewerData_WPF_APP.Models;
using ViewerData_WPF_APP.Services;

namespace ViewerData_WPF_APP.ViewModels;

public partial class GirdDataViewModel : ObservableObject
{
    private readonly IOperationServices _operationServices;
    private readonly IRabbitMqService _rabbitMqService;
    private readonly IModel _channel;
    public GirdDataViewModel(IOperationServices operationServices, IRabbitMqService rabbitMqService)
    {
        LoadedCommand = new AsyncRelayCommand(Loaded);
        UnloadedCommand = new AsyncRelayCommand(Unloaded);
        _operationServices = operationServices;
        _rabbitMqService = rabbitMqService;
        _channel = _rabbitMqService.CreateChannel().CreateModel();
    }

    public ICommand LoadedCommand { get; set; }
    public ICommand UnloadedCommand { get; set; }

    [ObservableProperty]
    private ObservableCollection<Operation> gridData; 

    private async Task Loaded()
    {
        await SubscribeQueue();
        await LoadData();
        await Task.CompletedTask;
    }

    private async Task SubscribeQueue()
    {
        _channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += DoJobFromQueue;

        _channel.BasicConsume(queue: "hello",
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

    private void DoJobFromQueue(AsyncEventingBasicConsumer consumer)
    {

    }

    private async Task Unloaded()
    {
        await Task.CompletedTask;
    }

    private async Task LoadData()
    {
        GridData = await _operationServices.GetOperations();
    }


}
