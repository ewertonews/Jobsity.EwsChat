namespace Jobsity.EwsChat.Server.DTO
{
    public class StockDto
    {
        public string Symbol { get; set; } = string.Empty;
        public decimal Open { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
    }
}
