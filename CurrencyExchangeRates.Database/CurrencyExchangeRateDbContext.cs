using CurrencyExchangeRates.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeRates.Database
{
    public class CurrencyExchangeRateDbContext : DbContext
    {
        public DbSet<CurrencyExchangeRate> CurrencyExchangeRates { get; set; }

        public CurrencyExchangeRateDbContext(DbContextOptions<CurrencyExchangeRateDbContext> options) : base(options)
        {
        }
    }
}
