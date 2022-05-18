using Microsoft.AspNetCore.SignalR;

namespace KitchenService
{
    public class OrderHub : Hub
    {
        public async Task SendMessage(Order order)
        {
            await Clients.All.SendAsync("ReceiveMessage", order);
        }
    }
}
