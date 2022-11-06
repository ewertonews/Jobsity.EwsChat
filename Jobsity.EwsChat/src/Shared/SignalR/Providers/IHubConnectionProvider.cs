using Microsoft.AspNetCore.SignalR.Client;

namespace Jobsity.EwsChat.Shared.SignalR.Providers
{
    public interface IHubConnectionProvider
    {
        HubConnection CreateConnection(Uri hubUrl);
    }
}
