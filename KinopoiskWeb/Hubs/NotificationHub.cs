using Microsoft.AspNetCore.SignalR;

namespace KinopoiskWeb.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message)
        {
            await Clients.Others.SendAsync("ReceiveNotification", message);
        }
    }

}
