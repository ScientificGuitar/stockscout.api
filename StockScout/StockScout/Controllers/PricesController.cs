using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockScout.Models;

namespace StockScout.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricesController : ControllerBase
    {
        private readonly PriceContext _context;

        public PricesController(PriceContext context)
        {
            _context = context;
        }

        // GET: api/Prices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Price>>> GetPrices()
        {
            return await _context.Prices.ToListAsync();
        }

        // GET: api/Prices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Price>> GetPrice(long id)
        {
            var price = await _context.Prices.FindAsync(id);

            if (price == null)
            {
                return NotFound();
            }

            return price;
        }

        // PUT: api/Prices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrice(long id, Price price)
        {
            if (id != price.PriceId)
            {
                return BadRequest();
            }

            _context.Entry(price).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Prices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Price>> PostPrice(Price price)
        {
            _context.Prices.Add(price);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrice), new { id = price.PriceId }, price);
        }

        // DELETE: api/Prices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrice(long id)
        {
            var price = await _context.Prices.FindAsync(id);
            if (price == null)
            {
                return NotFound();
            }

            _context.Prices.Remove(price);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PriceExists(long id)
        {
            return _context.Prices.Any(e => e.PriceId == id);
        }
    }
}
