using Jobsity.EwsChat.Shared;

namespace Jobsity.EwsChat.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private const int MaxNumberOfMessages = 50;
        private readonly Dictionary<string, Queue<Message>> _messages = new();

        public async Task Add(Message message)
        {
            if (_messages.ContainsKey(message.ChatRoomId))
            {
                var roomMessages = _messages[message.ChatRoomId];
                roomMessages.Enqueue(message);

                if (roomMessages.Count > MaxNumberOfMessages)
                {
                    roomMessages.Dequeue();
                }
            }
            else
            {
                var messages = new Queue<Message>();
                messages.Enqueue(message);
                _messages.Add(message.ChatRoomId, messages);
            }
            await Task.CompletedTask;
        }

        public async Task<IReadOnlyCollection<Message>> Get(string roomId)
        {
            var messagesList = new List<Message>();
            if (_messages.TryGetValue(roomId, out var messages))
            {
                messagesList = messages.ToList(); //ToListAsync it it was an IQueryable
            };

            return await Task.FromResult(messagesList);
        }
    }
}