using Jobsity.EwsChat.Server.Queuing;
using Jobsity.EwsChat.Server.Services;
using Jobsity.EwsChat.Shared;
using Jobsity.EwsChat.Shared.SignalR;

namespace Jobsity.EwsChat.Server.Tests.Services
{
    public class ChatBotServiceTests
    {
        private readonly AutoMocker _autoMocker = new();
        private const string BaseUrl = "https://ewapps.chatroom.com";
        private const string BotUsername = "Botsity";
        private Mock<IHubHandler> _chatHubHandler = null!;
        private Mock<IStockInfoRequestSender> _stockInfoRequestSender = null!;
        private Mock<ILoggingService> _loggingService = null!;
        private IChatBotService _chatBotService = null!;

        [SetUp]
        public void Setup()
        {
            _chatHubHandler = _autoMocker.GetMock<IHubHandler>();
            _stockInfoRequestSender = _autoMocker.GetMock<IStockInfoRequestSender>();
            _loggingService = _autoMocker.GetMock<ILoggingService>();
            _chatBotService = new ChatBotService(
                _chatHubHandler.Object,
                _stockInfoRequestSender.Object,
                _loggingService.Object,
                BaseUrl);
        }

        [Test]
        public void GivenWrongCommand_WhenProcessSpecialMessage_ThanSendMessageToChat()
        {
            //Arrange
            const string commandMessage = "/stoq=";
            const string expectedMessageToSend = "COMMAND NOT RECOGNIZED\nPlease use '/stock=[symbol]' to get stock share value.";

            //Act
            _chatBotService.ProcessSpecialMessage(commandMessage);

            //Assert
            _chatHubHandler.Verify(ch => ch.GetConnection(It.IsAny<System.Uri>(), It.IsAny<bool>()), Times.Once);
            _chatHubHandler.Verify(ch => ch.Send("AddMessage", BotUsername, expectedMessageToSend), Times.Once);
            _chatHubHandler.VerifyNoOtherCalls();
        }

        [Test]
        public void GivenCorrectCommandMessage_WhenProcessSpecialMessage_ThanSendMessageToQueue()
        {
            //Arrange
            const string commandMessage = "/stock=APPL.US";
            const string expectedMessageToSentToQueue = "APPL.US";

            //Act
            _chatBotService.ProcessSpecialMessage(commandMessage);

            //Assert
            _stockInfoRequestSender.Verify(s => s.SendStockInfoRequest(expectedMessageToSentToQueue), Times.Once);

        }

        [Test]
        [Ignore("Needs investigation - Causing other test to fail")]
        public void GivenExceptionThrow_WhenProcessSpencialMessage_ThanSendErrorMessageToChat()
        {
            //Arrange
            const string commandMessage = "/stock=APPL.US";
            const string exceptionMessage = "something bad occured!";
            const string botMessage = $"Sorry, something went wrong: {exceptionMessage}";
            const string logMessage = "Unable to process stock info request.";
            var exception = new Exception(exceptionMessage);

            _stockInfoRequestSender
                .Setup(s => s.SendStockInfoRequest(It.IsAny<string>())).Throws(exception);

            //Act
            _chatBotService.ProcessSpecialMessage(commandMessage);

            //Assert
            _loggingService.Verify(l => l.LogError(logMessage, exception));
            _chatHubHandler.Verify(ch => ch.GetConnection(It.IsAny<System.Uri>(), It.IsAny<bool>()), Times.Once);
            _chatHubHandler.Verify(ch => ch.Send("AddMessage", BotUsername, botMessage), Times.Once);
        }
    }
}
