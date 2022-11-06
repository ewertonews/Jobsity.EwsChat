namespace Jobsity.EwsChat.Server.Queuing.Options
{
    public class RabbitMqConfiguration
    {
        public string Hostname { get; set; } = null!;

        public string QueueName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
