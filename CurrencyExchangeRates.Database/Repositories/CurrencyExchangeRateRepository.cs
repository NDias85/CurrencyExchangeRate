using CurrencyExchangeRates.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchangeRates.Database.Repositories
{
    public class CurrencyExchangeRateRepository : ICurrencyExchangeRateRepository
    {
        private readonly CurrencyExchangeRateDbContext dbContext;

        public CurrencyExchangeRateRepository(CurrencyExchangeRateDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public async Task<CurrencyExchangeRate?> CreateCurrencyExchangeRateAsync(CurrencyExchangeRate currencyExchangeRate, CancellationToken cancellationToken)
        {
            var exists = await dbContext.CurrencyExchangeRates.AnyAsync(w => w.FromCurrencyCode == currencyExchangeRate.FromCurrencyCode && w.ToCurrencyCode == currencyExchangeRate.ToCurrencyCode, cancellationToken);
            if (exists) return null;

            await dbContext.AddAsync(currencyExchangeRate, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return currencyExchangeRate;
        }

        public async Task<bool> DeleteCurrencyExchangeRateAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken)
        {
            var entity = await dbContext.CurrencyExchangeRates.FirstOrDefaultAsync(w => w.FromCurrencyCode == currencyFrom && w.ToCurrencyCode == currencyTo, cancellationToken);
            if (entity == null) return false;

            dbContext.CurrencyExchangeRates.Remove(entity);

            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<CurrencyExchangeRate?> GetCurrencyExchangeRateAsync(string? currencyFrom, string? currencyTo, CancellationToken cancellationToken)
        {
            return await dbContext.CurrencyExchangeRates.FirstOrDefaultAsync(w => w.FromCurrencyCode == currencyFrom && w.ToCurrencyCode == currencyTo, cancellationToken);
        }

        public async Task<CurrencyExchangeRate?> UpdateCurrencyExchangeRateAsync(CurrencyExchangeRate currencyExchangeRate, CancellationToken cancellationToken)
        {
            var entity = await dbContext.CurrencyExchangeRates.FirstOrDefaultAsync(w => w.FromCurrencyCode == currencyExchangeRate.FromCurrencyCode && w.ToCurrencyCode == currencyExchangeRate.ToCurrencyCode, cancellationToken);
            if (entity == null) return null;

            currencyExchangeRate.Id = entity.Id;
            dbContext.CurrencyExchangeRates.Update(currencyExchangeRate);
            await dbContext.SaveChangesAsync(cancellationToken);

            return currencyExchangeRate;
        }
    }
}
