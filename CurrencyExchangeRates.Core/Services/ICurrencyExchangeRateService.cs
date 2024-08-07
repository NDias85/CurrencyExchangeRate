using CurrencyExchangeRates.Models.DTO;

namespace CurrencyExchangeRates.Core.Services
{
    public interface ICurrencyExchangeRateService
    {
        /// <summary>
        /// Gets the CurrencyExchangeRate Dto by currency pair asynchronously.
        /// </summary>
        /// <param name="currencyFrom"></param>
        /// <param name="currencyTo"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>nullable CurrencyExchangeRateDto</returns>
        Task<CurrencyExchangeRateDto?> GetCurrencyExchangeRateAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a new CurrencyExchangeRate entity asynchronously.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>nullable CurrencyExchangeRateDto</returns>
        Task<CurrencyExchangeRateDto> CreateCurrencyExchangeRateAsync(CurrencyExchangeRateDto request, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a CurrencyExchangeRate entity asynchronously.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>nullable CurrencyExchangeRateDto</returns>
        Task<CurrencyExchangeRateDto?> UpdateCurrencyExchangeRateAsync(CurrencyExchangeRateDto request, CancellationToken cancellationToken);

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
