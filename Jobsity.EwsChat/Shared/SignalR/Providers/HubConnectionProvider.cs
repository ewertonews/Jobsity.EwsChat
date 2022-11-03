using Microsoft.AspNetCore.SignalR.Client;

namespace Jobsity.EwsChat.Shared.SignalR.Providers
{
    public class HubConnectionProvider : IHubConnectionProvider
    {
        public HubConnection CreateConnection(Uri hubUrl)
        {
            return new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();
        }
    }
}
