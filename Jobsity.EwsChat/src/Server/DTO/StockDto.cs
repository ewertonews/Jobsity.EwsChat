namespace Jobsity.EwsChat.Server.DTO
{
    public class StockDto
    {
        public string Symbol { get; set; } = string.Empty;
        public string Open { get; set; } = string.Empty;
        public string Low { get; set; } = string.Empty;
        public string Close { get; set; } = string.Empty;

        public override string ToString()
        {
            return Close != "N/D" 
                ? $"{Symbol.ToUpperInvariant()} quote is ${Close} per share." 
                : "Sorry the information about this stock is unavailable in the Stooq API.";
        }
    }
}
