using Jobsity.EwsChat.Shared.SignalR.Extensions;
using Jobsity.EwsChat.Shared.SignalR.Providers;
using Microsoft.AspNetCore.SignalR.Client;

namespace Jobsity.EwsChat.Shared.SignalR
{
    public class ChatHubHandler : IHubHandler
    {

        private HubConnection _hubConnection = null!;
        private readonly IHubConnectionProvider _hubConnectionProvider;

        public ChatHubHandler(IHubConnectionProvider hubConnectionProvider)
        {
            _hubConnectionProvider = hubConnectionProvider;
        }

        public bool IsConnected() => _hubConnection.State == HubConnectionState.Connected;

        public HubConnection GetConnection(Uri? chatUri = null)
        {
            chatUri ??= new Uri("/chathub");
            return _hubConnection ??= _hubConnectionProvider.CreateConnection(chatUri);
        }

        public Task StablishHubConnection()
        {
            throw new NotImplementedException();
        }

        public void SetMethodNameAndHandler<T>(string methodName, Action<T, T> messageHandler)
        {
            //methodName = ReceiveMessage
            _hubConnection.HandleAction(methodName, messageHandler);
        }

        public async Task Send(string methodName, object? arg1, object? arg2)
        {
            //method name: AddMessage
            await _hubConnection.SendAsync(methodName, arg1, arg2);
        }
    }
}
