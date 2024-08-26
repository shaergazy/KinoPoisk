using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace KinopoiskWeb.Hubs
{
    [Authorize]
    public class SupportChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> userConnections = new ConcurrentDictionary<string, string>();
        private static List<string> adminConnections = new List<string>();

        public override async Task OnConnectedAsync()
        {
            var user = Context.User.Identity.Name;

            if (Context.User.IsInRole("Admin"))
            {
                adminConnections.Add(Context.ConnectionId);
                Console.WriteLine($"Admin connected: {user} with ConnectionId: {Context.ConnectionId}");
            }
            else
            {
                userConnections[user] = Context.ConnectionId;
                Console.WriteLine($"User connected: {user} with ConnectionId: {Context.ConnectionId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.IsInRole("Admin"))
            {
                adminConnections.Remove(Context.ConnectionId);
            }
            var user = Context.User.Identity.Name;
            userConnections.TryRemove(user, out _);

            Console.WriteLine($"User disconnected: {user}");

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToAdmin(string message)
        {
            var user = Context.User.Identity.Name;

            if (adminConnections.Any())
            {
                Console.WriteLine($"Sending message from {user} to Admins: {message}");
                foreach(var connection in adminConnections)
                {
                    if(connection != null)
                    {
                        await Clients.Client(connection).SendAsync("ReceiveMessage", user, message);
                    }
                }
            }
            else
            {
                Console.WriteLine("No admin connected.");
            }
        }

        public async Task SendMessageToUser(string userName, string message)
        {
            if (Context.User.IsInRole("Admin"))
            {
                var userConnectionId = userConnections.GetValueOrDefault(userName);

                if (userConnectionId != null)
                {
                    Console.WriteLine($"Admin sending message to {userName}: {message}");
                    await Clients.Client(userConnectionId).SendAsync("ReceiveMessage", "Admin", message);
                }
                else
                {
                    Console.WriteLine($"User {userName} not found.");
                }
            }
            else
            {
                Console.WriteLine("Unauthorized attempt to send message to user.");
            }
        }

        public async Task GetActiveUsers()
        {
            var users = userConnections.Keys.ToList();
            await Clients.Caller.SendAsync("ReceiveActiveUsers", users);
        }

    }
}
