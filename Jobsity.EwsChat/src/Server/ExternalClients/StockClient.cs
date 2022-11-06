using CsvHelper;
using Jobsity.EwsChat.Server.DTO;
using Jobsity.EwsChat.Shared;
using System.Globalization;

namespace Jobsity.EwsChat.Server.ExternalClients
{
    public class StockClient : IStockClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILoggingService _loggingService;

        public StockClient(HttpClient httpClient, ILoggingService loggingService)
        {
            _httpClient = httpClient;
            _loggingService = loggingService;
        }

        public async Task<StockDto> GetStockInfo(string stockSymbol)
        {

            var stockUri = new Uri(_httpClient.BaseAddress, $"/q/l/?s={stockSymbol}&f=sd2t2ohlcv&h&e=csv");
            var request = new HttpRequestMessage(HttpMethod.Get, stockUri);
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var stock = new StockDto();

            try
            {
                var content = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(content);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                var records = csv.GetRecords<StockDto>();

                if (records == null)
                {
                    return stock;
                }

                var recordsList = records.ToList();
                stock = recordsList.Any() ? recordsList[0] : stock;

            }
            catch (CsvHelper.CsvHelperException csvhex)
            {
                _loggingService.LogError("Unable to parse content of stock information from retrieved CSV.", csvhex);
                throw;
            }

            return stock;
        }

    }
}
