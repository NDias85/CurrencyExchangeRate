using CurrencyExchangeRates.Models.Entities;

namespace CurrencyExchangeRates.Database.Repositories
{
    public interface ICurrencyExchangeRateRepository
    {
        Task<CurrencyExchangeRate?> GetCurrencyExchangeRateAsync(string? currencyFrom, string? currencyTo, CancellationToken cancellationToken);

        Task<CurrencyExchangeRate?> CreateCurrencyExchangeRateAsync(CurrencyExchangeRate currencyExchangeRate, CancellationToken cancellationToken);

        Task<CurrencyExchangeRate?> UpdateCurrencyExchangeRateAsync(CurrencyExchangeRate currencyExchangeRate, CancellationToken cancellationToken);

        Task<bool> DeleteCurrencyExchangeRateAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken);
    }
}
