using Microsoft.AspNetCore.SignalR;

namespace Jobsity.EwsChat.Server.SignalRHubs
{
    //[Authorize]?
    public class ChatHub : Hub
    {
        private static readonly Dictionary<string, string> Users = new();

        public override async Task OnConnectedAsync()
        {
            string user = Context.GetHttpContext()?.Request.Query["user"] ?? "New user";

            if (user != null && !Users.ContainsKey(user))
            {
                Users.Add(Context.ConnectionId, user);
            }

            await AddMessage(string.Empty, $"{user} joined the chat.");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var user = Users.FirstOrDefault(userEntry => userEntry.Key == Context.ConnectionId).Value;
            await AddMessage(string.Empty, $"{user} has left the chat.");
            Users.Remove(user);

        }

        public async Task AddMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
