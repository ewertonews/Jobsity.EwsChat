namespace Jobsity.EwsChat.Server.Queuing
{
    public interface IStockInfoRequestSender
    {
        void SendStockInfoRequest(string stockSymbol);
    }
}
