using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    public class TelemetryHub : Hub
    {
        public async Task JoinDevice(string deviceId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                deviceId
            );
        }
    }
}
