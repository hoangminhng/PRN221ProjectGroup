using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.Hubs
{
    public class OnlineHub : Hub
    {
        public async Task UserConnected(int userId)
        {
            await Clients.Others.SendAsync("UserConnected", userId);
        }

        public async Task UserDisconnected(int userId)
        {
            await Clients.Others.SendAsync("UserDisconnected", userId);
        }
    }
}
