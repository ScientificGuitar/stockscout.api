namespace StockScout.Models
{
    public class Stock
    {
        public long StockId { get; set; }
        public string? Symbol { get; set; }
        public string? Name { get; set; }
        public string? Exchange { get; set; }
    }
}
