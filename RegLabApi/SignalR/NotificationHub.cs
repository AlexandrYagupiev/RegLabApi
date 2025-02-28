using Microsoft.AspNetCore.SignalR;

namespace RegLabApi.SignalR
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            try
            {
                await Clients.All.SendAsync("ПолучитьСообщение", user, message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка отправки уведомления.", ex);
            }
        }

        public async Task SubscribeToEvents(string eventType)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, eventType);
            }
            catch (Exception ex)
            {
                throw new Exception("При добавлений в группу произошла ошибка", ex);
            }
        }

        public async Task UnsubscribeFromEvents(string eventType)
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, eventType);
            }
            catch (Exception ex)
            {
                throw new Exception("При удалении в группу произошла ошибка", ex);
            }
        }
    }
}
