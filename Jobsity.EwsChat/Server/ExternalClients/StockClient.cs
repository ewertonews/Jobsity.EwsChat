using CsvHelper;
using Jobsity.EwsChat.Server.DTO;
using System.Globalization;

namespace Jobsity.EwsChat.Server.ExternalClients
{
    public class StockClient : IStockClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<StockClient> _logger;

        public StockClient(HttpClient httpClient, ILogger<StockClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<StockDto> GetStockInfo(string stockSymbol)
        {

            var stockUri = new Uri($"/q/l/?s={stockSymbol}&f=sd2t2ohlcv&h&e=csv");
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
            catch (CsvHelperException csvhex)
            {
                _logger.LogError("Unable to parse content of stock information from retrieved CSV.", csvhex);
            }

            return stock;
        }

    }
}
