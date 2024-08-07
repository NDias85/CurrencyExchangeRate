using CurrencyExchangeRates.Models.Entities;

namespace CurrencyExchangeRates.Database.Repositories
{
    public interface ICurrencyExchangeRateRepository
    {
        /// <summary>
        /// Gets the CurrencyExchangeRate entity by currency pair asynchronously.
        /// </summary>
        /// <param name="currencyFrom"></param>
        /// <param name="currencyTo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>nullable CurrencyExchangeRate</returns>
        Task<CurrencyExchangeRate?> GetCurrencyExchangeRateAsync(string? currencyFrom, string? currencyTo, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new CurrencyExchangeRate entity asynchronously.
        /// </summary>
        /// <param name="currencyExchangeRate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>nullable CurrencyExchangeRate</returns>
        Task<CurrencyExchangeRate?> CreateCurrencyExchangeRateAsync(CurrencyExchangeRate currencyExchangeRate, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a CurrencyExchangeRate entity asynchronously.
        /// </summary>
        /// <param name="currencyExchangeRate"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<CurrencyExchangeRate?> UpdateCurrencyExchangeRateAsync(CurrencyExchangeRate currencyExchangeRate, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a CurrencyExchangeRate entity by currency pair asynchronously.
        /// </summary>
        /// <param name="currencyFrom"></param>
        /// <param name="currencyTo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>bool</returns>
        Task<bool> DeleteCurrencyExchangeRateAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken);
    }
}
