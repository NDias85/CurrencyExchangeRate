using CurrencyExchangeRates.Models.DTO;

namespace CurrencyExchangeRates.Core.Services
{
    public interface ICurrencyExchangeRateService
    {
        Task<CurrencyExchangeRateDto?> GetCurrencyExchangeRateAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken);

        Task<CurrencyExchangeRateDto> CreateCurrencyExchangeRateAsync(CurrencyExchangeRateDto request, CancellationToken cancellationToken);

        Task<CurrencyExchangeRateDto?> UpdateCurrencyExchangeRateAsync(CurrencyExchangeRateDto request, CancellationToken cancellationToken);

        Task<bool> DeleteCurrencyExchangeRateAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken);
    }
}
