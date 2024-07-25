namespace StockScout.Models
{
    public class Stock
    {
        public long StockId { get; set; }
        public string? Ticker { get; set; } = null!;
    }
}
