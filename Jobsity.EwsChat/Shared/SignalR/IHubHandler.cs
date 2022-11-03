using Microsoft.AspNetCore.SignalR.Client;

namespace Jobsity.EwsChat.Shared.SignalR
{
    public interface IHubHandler
    {
        bool IsConnected();
        HubConnection GetConnection();
        Task StablishHubConnection();
        void SetMethodNameAndHandler<T>(string menthodName, Action<T, T> messageHandler);
        Task Send(string methodName, object? arg1, object? arg2);
    }
}
