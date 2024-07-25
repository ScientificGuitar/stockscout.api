namespace StockScout.Models
{
    public class Price
    {
        public long PriceId { get; set; }
        public long StockId { get; set; }
        public decimal PriceAmount { get; set; }
        public DateTime TmStamp { get; set; }
    }
}
