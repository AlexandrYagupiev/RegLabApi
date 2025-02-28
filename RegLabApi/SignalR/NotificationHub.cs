using Microsoft.AspNetCore.SignalR;
using RegLabApi.Services;

namespace RegLabApi.SignalR
{
    public class NotificationHub : Hub
    {
        private readonly IConfigurationService _configurationService;

        public NotificationHub(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task SendMessage(string group, string message)
        {
            try
            {
                await Clients.Group(group).SendAsync("ПолучитьСообщение", message);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка отправки уведомления.", ex);
            }
        }

        public async Task SubscribeToGroup(string group)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, group);
            }
            catch (Exception ex)
            {
                throw new Exception("При добавлений в группу произошла ошибка", ex);
            }
        }

        public async Task UnsubscribeFromGroup(string group)
        {
            try
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
            }
            catch (Exception ex)
            {
                throw new Exception("При удалении из группы произошла ошибка", ex);
            }
        }

        public async Task SendFullConfigurationList()
        {
            var configurations = _configurationService.GetAll();
            await Clients.Caller.SendAsync("ПолныйСписокКонфигураций", configurations);
        }
    }
}
