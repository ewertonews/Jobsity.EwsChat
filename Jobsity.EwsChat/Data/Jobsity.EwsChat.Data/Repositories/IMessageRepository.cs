using Jobsity.EwsChat.Shared;

namespace Jobsity.EwsChat.Data.Repositories
{
    public interface IMessageRepository
    {
        Task Add(Message message);
        Task<IReadOnlyCollection<Message>> Get(string roomId);
    }
}
