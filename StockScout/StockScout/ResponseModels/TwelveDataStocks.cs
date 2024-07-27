using Newtonsoft.Json;

namespace StockScout.ResponseModels
{
    public class TwelveDataStocks
    {
        public List<TwelveDataStockData> Data { get; set; } = null!;
        public int Count { get; set; }
        public string Status { get; set; }
    }

    public class TwelveDataStockData
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public string Exchange { get; set; }
        // TODO: Remove underscore here and correctly deserialize from snake_case to CamelCase
        public string Mic_Code { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
    }
}
