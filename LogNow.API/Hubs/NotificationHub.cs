using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LogNow.API.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var username = Context.User?.Identity?.Name ?? "Anonymous";
        await Clients.All.SendAsync("UserConnected", username);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var username = Context.User?.Identity?.Name ?? "Anonymous";
        await Clients.All.SendAsync("UserDisconnected", username);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("ReceiveNotification", message);
    }

    public async Task JoinIncidentGroup(string incidentId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"incident_{incidentId}");
    }

    public async Task LeaveIncidentGroup(string incidentId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"incident_{incidentId}");
    }
}
