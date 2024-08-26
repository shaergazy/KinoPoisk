using DAL.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace KinopoiskWeb.Hubs
{
    [Authorize]
    public class SupportChatHub : Hub
    {
        private static ConcurrentDictionary<string, string> adminConnections = new ConcurrentDictionary<string, string>();
        private readonly UserManager<User> _userManager;

        public SupportChatHub(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public override async Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;

            if (Context.User.IsInRole("Admin"))
            {
                adminConnections.TryAdd(userName, Context.UserIdentifier);

                Console.WriteLine($"Admin connected: {userName} with ConnectionId: {Context.ConnectionId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToAdmin(string message)
        {
            var userId = Context.User.Identity.Name;

            if (adminConnections.Any())
            {
                Console.WriteLine($"Sending message from {userId} to Admins: {message}");

                foreach (var adminId in adminConnections.Values)
                {
                    await Clients.User(adminId).SendAsync("ReceiveMessage", userId, message);
                }
            }
            else
            {
                Console.WriteLine("No admin connected.");
            }
        }

        public async Task SendMessageToUser(string userName, string message)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var suser = Clients.User(user.Id);
            if (Context.User.IsInRole("Admin"))
            {
                Console.WriteLine($"Admin sending message to {userName}: {message}");
                await Clients.User(user.Id).SendAsync("ReceiveMessage", "Admin", message);
            }
            else
            {
                Console.WriteLine("Unauthorized attempt to send message to user.");
            }
        }
    }
}
