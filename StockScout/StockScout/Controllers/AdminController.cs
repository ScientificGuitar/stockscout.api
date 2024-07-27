using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockScout.Models;
using StockScout.ResponseModels;
using static System.Net.WebRequestMethods;

namespace StockScout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly PriceContext _priceContext;
        private readonly StockContext _stockContext;

        public AdminController(PriceContext priceContext, StockContext stockContext)
        {
            _priceContext = priceContext;
            _stockContext = stockContext;
        }

        [HttpPost("populatestocks")]
        public async Task PopulateStocks()
        {
            string url = "https://api.twelvedata.com/stocks?apikey=a310a70999fb48f88f2253a447d22c98";
            using (HttpClient client = new HttpClient())
            {
                // Last time this was checked, the call returned 141305 stocks
                var response = await client.GetFromJsonAsync<TwelveDataStocks>(url);
                if (response?.Data == null)
                    return;

                foreach (var stock in response.Data)
                {
                    _stockContext.Stocks.Add(new Stock
                    {
                        Symbol = stock.Symbol,
                        Name= stock.Name,
                        Exchange = stock.Exchange
                    });
                }

                await _stockContext.SaveChangesAsync();
            }
        }

        [HttpPost("populateprices")]
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

                    await _priceContext.SaveChangesAsync();
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

            _priceContext.Prices.Add(price);
        }
    }
}


