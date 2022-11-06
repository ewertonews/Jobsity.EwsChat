using Jobsity.EwsChat.Shared.SignalR.Extensions;
using Jobsity.EwsChat.Shared.SignalR.Providers;
using Microsoft.AspNetCore.SignalR.Client;

namespace Jobsity.EwsChat.Shared.SignalR
{
    public class ChatHubHandler : IHubHandler
    {

        private HubConnection _hubConnection;
        private readonly IHubConnectionProvider _hubConnectionProvider;
        private readonly ILoggingService _loggingService;

        public ChatHubHandler(IHubConnectionProvider hubConnectionProvider, ILoggingService loggingService)
        {
            _hubConnectionProvider = hubConnectionProvider;
            _loggingService = loggingService;
        }

        public bool IsConnected() => _hubConnection.State == HubConnectionState.Connected;

        public async Task<HubConnection> GetConnection(Uri? hubUri = null, bool start = true)
        {
            hubUri ??= new Uri("/chathub");
            _hubConnection ??= _hubConnectionProvider.CreateConnection(hubUri);

            try
            {
                if (start && !IsConnected())
                {
                    await _hubConnection.StartAsync();
                }
            }
            catch (Exception exception)
            {
                _loggingService.LogError("Unable to create connection with the chat hub.", exception);
                throw;
            }
            

            return _hubConnection;
        }

        public async Task StablishHubConnection()
        {
            await _hubConnection.StartAsync();
        }

        public void SetMethodNameAndHandler<T>(string methodName, Action<T, T> messageHandler)
        {
            _hubConnection.HandleAction(methodName, messageHandler);
        }

        public async Task Send(string methodName, object? arg1, object? arg2)
        {
            await _hubConnection.SendAsync(methodName, arg1, arg2);
        }
    }
}
