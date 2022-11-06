using Jobsity.EwsChat.Shared;

namespace Jobsity.EwsChat.Server.Services
{
    public interface IChatBotService
    {
        Task ProcessSpecialMessage(string message);
        Task SendMessageToChat(string message);
    }
}
