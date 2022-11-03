using Jobsity.EwsChat.Shared;

namespace Jobsity.EwsChat.Server.Services
{
    public interface IMessagesService
    {
        Task<string> ProcessSpecialMessage(string message);
    }
}
