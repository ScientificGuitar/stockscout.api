using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockScout.Models;

namespace StockScout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly PriceContext _context;

        public AdminController(PriceContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task PopulatePrices(int stockId, string stockName)
        {
            string url = $"https://www.alphavantage.co/query?symbol={stockName}&function=TIME_SERIES_DAILY&apikey=KNXYECX8VRFZ9N7W&outputsize=full";
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var jsonString = await response.Content.ReadAsStringAsync();
                var stockData = JObject.Parse(jsonString);

                var data = (JObject?)stockData["Time Series (Daily)"];
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        InsertStockPrice(stockId, item);
                    }

                    await _context.SaveChangesAsync();
                }
            }
        }

        private void InsertStockPrice(int stockId, KeyValuePair<string, JToken?> item)
        {
            var priceData = JObject.Parse(item.Value?.ToString() ?? string.Empty);
            var closePrice = priceData["4. close"]?.ToString() ?? string.Empty;
            Price price = new Price()
            {
                StockId = stockId,
                TmStamp = DateTime.Parse(item.Key),
                PriceAmount = decimal.Parse(closePrice)
            };

            _context.Prices.Add(price);
        }
    }
}


