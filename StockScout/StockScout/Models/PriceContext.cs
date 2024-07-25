using Microsoft.EntityFrameworkCore;

namespace StockScout.Models
{
    public class PriceContext : DbContext
    {
        public PriceContext(DbContextOptions<PriceContext> options) : base(options)
        {

        }

        public DbSet<Price> Prices { get; set; }
    }
}
