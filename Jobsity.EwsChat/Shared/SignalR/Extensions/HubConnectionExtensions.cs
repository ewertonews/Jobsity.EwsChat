using Microsoft.AspNetCore.SignalR.Client;

namespace Jobsity.EwsChat.Shared.SignalR.Extensions
{
    public static class HubConnectionExtensions
    {
        public static void HandleAction<T1, T2>(
            this HubConnection hubConnection, string methodName, Action<T1, T2> handler)
        {
            hubConnection.On<T1, T2>(methodName, handler);
        }
    }
}
