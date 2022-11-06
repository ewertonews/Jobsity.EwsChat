using Jobsity.EwsChat.Server.Queuing;
using Jobsity.EwsChat.Shared;
using Jobsity.EwsChat.Shared.SignalR;

namespace Jobsity.EwsChat.Server.Services
{
    public class ChatBotService : IChatBotService
    {
        private const string BotUsername = "Botsity";
        private const string GetStockInfoCommand = @"/stock=";
        private readonly IHubHandler _chatHubHandler;
        private readonly IStockInfoRequestSender _stockInfoRequestSender;
        private readonly ILoggingService _loggingService;
        private readonly string _baseUrl;

        public ChatBotService(
            IHubHandler chatHubHandler,
            IStockInfoRequestSender stockInfoRequestSender,
            ILoggingService loggingService,
            string baseUrl)
        {
            _chatHubHandler = chatHubHandler;
            _stockInfoRequestSender = stockInfoRequestSender;
            _baseUrl = baseUrl;
            _loggingService = loggingService;
        }

        public async Task ProcessSpecialMessage(string message)
        {
            try
            {
                if (!message.StartsWith(GetStockInfoCommand))
                {
                    const string botMessage = "COMMAND NOT RECOGNIZED\nPlease use '/stock=[symbol]' to get stock share value.";
                    await SendMessageToChat(botMessage);
                    return;
                }

                message = message.Replace(GetStockInfoCommand, "");
                _stockInfoRequestSender.SendStockInfoRequest(message);
            }
            catch (Exception exception)
            {
                _loggingService.LogError("Unable to process stock info request.", exception);
                var botMessage = $"Sorry, something went wrong: {exception.Message}";
                await SendMessageToChat(botMessage);
            }
        }

        public async Task SendMessageToChat(string message)
        {
            var hubUri = new Uri($"{_baseUrl}/chathub?user={BotUsername}");
            await _chatHubHandler.GetConnection(hubUri, start: true);
            await _chatHubHandler.Send("AddMessage", BotUsername, message);
        }
    }
}
