using Jobsity.EwsChat.Server.Queuing.Options;
using Jobsity.EwsChat.Shared;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace Jobsity.EwsChat.Server.Queuing
{
    public class StockInfoRequestSender : IStockInfoRequestSender
    {
        private readonly string _hostName;
        private readonly string _queueName;
        private readonly string _userName;
        private readonly string _password;
        private IConnection _connection;
        private readonly ILoggingService _loggingService;


        public StockInfoRequestSender(IOptions<RabbitMqConfiguration> rabbitMqOptions, ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _queueName = rabbitMqOptions.Value.QueueName;
            _hostName = rabbitMqOptions.Value.Hostname;
            _userName = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;

            _connection = CreateConnection();
        }

        public void SendStockInfoRequest(string stockSymbol)
        {
            try
            {
                _connection ??= CreateConnection();

                using var channel = _connection.CreateModel();
                channel.QueueDeclare(_queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(stockSymbol);
                channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
            }
            catch (Exception exception)
            {
                _loggingService.LogError($"Unable to send message to queue: {_queueName}", exception);
                throw;
            }

        }

        private IConnection CreateConnection()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };
            return connectionFactory.CreateConnection();
        }

    }
}
