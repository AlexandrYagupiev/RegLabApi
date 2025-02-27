using Microsoft.AspNetCore.SignalR;

namespace RegLabApi.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ПолучитьСообщение", user, message);
        }

        public async Task SubscribeToEvents(string eventType)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, eventType);
        }

        public async Task UnsubscribeFromEvents(string eventType)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, eventType);
        }
    }
}
