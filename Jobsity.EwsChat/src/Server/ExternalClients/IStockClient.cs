using Jobsity.EwsChat.Server.DTO;

namespace Jobsity.EwsChat.Server.ExternalClients
{
    public interface IStockClient
    {
        Task<StockDto> GetStockInfo(string stockSymbol);
    }
}
