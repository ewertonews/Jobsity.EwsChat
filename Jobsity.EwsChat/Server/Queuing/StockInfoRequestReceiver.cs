using Jobsity.EwsChat.Server.Queuing.Options;
using Jobsity.EwsChat.Server.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Jobsity.EwsChat.Server.ExternalClients;
using Jobsity.EwsChat.Shared;

namespace Jobsity.EwsChat.Server.Queuing
{
    public class StockInfoRequestReceiver : BackgroundService
    {
        private IModel? _channel;
        private IConnection? _connection;
        private readonly IChatBotService _chatBotService;
        private readonly IStockClient _stockClient;
        private readonly ILoggingService _loggingService;

        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public StockInfoRequestReceiver(
            IChatBotService chatBotService,
            IStockClient stockClient,
            IOptions<RabbitMqConfiguration> rabbitMqOptions, 
            ILoggingService loggingService)
        {
            _chatBotService = chatBotService;
            _stockClient = stockClient;
            _loggingService = loggingService;
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;

            InitializeRabbitMqListener();
        }


        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                stoppingToken.ThrowIfCancellationRequested();

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (obj, evtArgs) =>
                {
                    var message = Encoding.UTF8.GetString(evtArgs.Body.ToArray());

                    await HandleMessage(message);

                    _channel?.BasicAck(evtArgs.DeliveryTag, false);
                };

                _ = _channel.BasicConsume(_queueName, false, consumer);
                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                _loggingService.LogError($"Unable to consume from the queue: {_queueName}", exception);
            }
           
        }

        private async Task HandleMessage(string message)
        {
            var stock = await _stockClient.GetStockInfo(message);
            await _chatBotService.SendMessageToChat(stock.ToString());
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }

    }
}
