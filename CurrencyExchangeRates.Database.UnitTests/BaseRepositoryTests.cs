using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeRates.Database.UnitTests
{
    public class BaseRepositoryTests
    {
        protected const string InMemoryConnString = "DataSource=:memory:";
        protected CurrencyExchangeRateDbContext _context;

        public BaseRepositoryTests()
        {
            // Create a SQLLite in memory connection string to create a context with
            var connection = new SqliteConnection(InMemoryConnString);
            connection.CreateFunction("newid", () => Guid.NewGuid());

            var options = new DbContextOptionsBuilder<CurrencyExchangeRateDbContext>().UseSqlite(connection).Options;
            connection.Open();

            _context = new CurrencyExchangeRateDbContext(options);

            // Create the database
            _context.Database.EnsureCreated();
        }
    }
}