namespace Jobsity.EwsChat.Shared
{
    public class Message
    {
        private Message(string text, string chatRoomId)
        {
            Text = text;
            ChatRoomId = chatRoomId;
        }

        public string Text { get; }
        public TimeOnly SentAt { get; private init; }
        public string ChatRoomId { get; set; }

        public override string ToString()
        {
            return $"[{SentAt:H:mm:ss}] {Text}";
        }

        public static class Factory
        {
            public static Message Create(string text, string chatRoomId)
            {
                if (string.IsNullOrEmpty(text))
                {
                    throw new ArgumentException("cannot be null or empty;", nameof(text));
                }

                if (string.IsNullOrEmpty(chatRoomId))
                {
                    throw new ArgumentException("cannot be null or empty;", nameof(chatRoomId));
                }

                var currentDate = DateTime.Now;
                var message = new Message(text, chatRoomId)
                {
                    SentAt = new TimeOnly(currentDate.Hour, currentDate.Minute)
                };
                return message;
            }
        }
    }
}
