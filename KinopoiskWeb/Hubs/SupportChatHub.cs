using DAL.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace KinopoiskWeb.Hubs
{
    [Authorize]
    public class SupportChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> userConnections = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, string> adminConnections = new ConcurrentDictionary<string, string>();
        private readonly ILogger<SupportChatHub> _logger;

        public SupportChatHub(ILogger<SupportChatHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;

            if (Context.User.IsInRole("Admin"))
            {
                adminConnections.AddOrUpdate(userName, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);
                _logger.LogInformation("Admin connected: {UserName} with ConnectionId: {ConnectionId}", userName, Context.ConnectionId);
            }
            else
            {
                userConnections.AddOrUpdate(userName, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);
                _logger.LogInformation("User connected: {UserName} with ConnectionId: {ConnectionId}", userName, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User.Identity.Name;

            if (Context.User.IsInRole("Admin"))
            {
                adminConnections.TryRemove(userName, out _);
                _logger.LogInformation("Admin disconnected: {UserName}", userName);
            }
            else
            {
                userConnections.TryRemove(userName, out _);
                _logger.LogInformation("User disconnected: {UserName}", userName);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToAdmin(string message)
        {
            var userName = Context.User.Identity.Name;

            if (adminConnections.Any())
            {
                _logger.LogInformation("Sending message from {UserName} to Admins: {Message}", userName, message);

                foreach (var adminConnection in adminConnections.Values)
                {
                    await Clients.Client(adminConnection).SendAsync("ReceiveMessage", userName, message);
                }
            }
            else
            {
                _logger.LogWarning("No admin connected.");
            }
        }

        public async Task SendMessageToUser(string userName, string message)
        {
            if (Context.User.IsInRole("Admin"))
            {
                if (userConnections.TryGetValue(userName, out var connection))
                {
                    _logger.LogInformation("Admin sending message to {UserName}: {Message}", userName, message);
                    await Clients.Client(connection).SendAsync("ReceiveMessage", "Admin", message);
                }
                else
                {
                    _logger.LogWarning("User {UserName} is not connected.", userName);
                }
            }
            else
            {
                _logger.LogWarning("Unauthorized attempt to send message to user.");
            }
        }
    }
}
