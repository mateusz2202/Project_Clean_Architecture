using Microsoft.AspNetCore.SignalR;

namespace HH_ASP_APP.Hubs;

public class RabbitMQHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
