using AutoMapper;
using CurrencyExchangeRates.Database.Repositories;
using CurrencyExchangeRates.Models.DTO;
using CurrencyExchangeRates.Models.Entities;
using CurrencyExchangeRates.Models.External;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace CurrencyExchangeRates.Core.Services
{
    public class CurrencyExchangeRateService : ICurrencyExchangeRateService
    {
        private readonly ILogger<CurrencyExchangeRateService> _logger;
        private readonly ICurrencyExchangeRateRepository _currencyExchangeRateRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        public CurrencyExchangeRateService(ILogger<CurrencyExchangeRateService> logger, ICurrencyExchangeRateRepository currencyExchangeRateRepository, IMapper mapper, HttpClient httpClient)
        {
            if (httpClient == null || httpClient.BaseAddress == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            _logger = logger;
            _currencyExchangeRateRepository = currencyExchangeRateRepository ?? throw new ArgumentNullException(nameof(currencyExchangeRateRepository));
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<CurrencyExchangeRateDto> CreateCurrencyExchangeRateAsync(CurrencyExchangeRateDto request, CancellationToken cancellationToken)
        {
            var newEntity = _mapper.Map<CurrencyExchangeRate>(request);
            var result = await _currencyExchangeRateRepository.CreateCurrencyExchangeRateAsync(newEntity, cancellationToken);

            // TODO: Send event message

            return request;
        }

        public async Task<bool> DeleteCurrencyExchangeRateAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken)
        {
            return await _currencyExchangeRateRepository.DeleteCurrencyExchangeRateAsync(currencyFrom, currencyTo, cancellationToken);
        }

        public async Task<CurrencyExchangeRateDto?> GetCurrencyExchangeRateAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken)
        {
            var entity = await _currencyExchangeRateRepository.GetCurrencyExchangeRateAsync(currencyFrom, currencyTo, cancellationToken);
            if (entity != null)
            {
                return _mapper.Map<CurrencyExchangeRateDto>(entity);
            }

            // TODO: Integration with https://www.alphavantage.co/documentation/#currency-exchange

            var url = _httpClient.BaseAddress!.AbsoluteUri;
            url = QueryHelpers.AddQueryString(url, "function", "CURRENCY_EXCHANGE_RATE");
            url = QueryHelpers.AddQueryString(url, "from_currency", currencyFrom);
            url = QueryHelpers.AddQueryString(url, "to_currency", currencyTo);

            BaseResponse? response;
            try
            {
                response = await _httpClient.GetFromJsonAsync<BaseResponse>(url);
                if (response == null || response.RealTimeCurrencyExchangeRate == null)
                {
                    _logger.LogError("Error parsing response from AlphaVantage service.");
                    return null;
                }
            } catch (Exception ex) 
            {
                _logger.LogError(ex, "Error getting response from AlphaVantage service.");
                throw;
            }

            var newEntity = _mapper.Map<CurrencyExchangeRate>(response.RealTimeCurrencyExchangeRate);
            newEntity = await _currencyExchangeRateRepository.CreateCurrencyExchangeRateAsync(newEntity, cancellationToken);

            // TODO: Send event message

            return _mapper.Map<CurrencyExchangeRateDto>(newEntity);
        }

        public async Task<CurrencyExchangeRateDto?> UpdateCurrencyExchangeRateAsync(CurrencyExchangeRateDto request, CancellationToken cancellationToken)
        {
            var entity = await _currencyExchangeRateRepository.GetCurrencyExchangeRateAsync(request.FromCurrencyCode, request.ToCurrencyCode, cancellationToken);
            if (entity == null)
            {
                return null;
            }

            entity = _mapper.Map(request, entity);

            entity = await _currencyExchangeRateRepository.UpdateCurrencyExchangeRateAsync(entity, cancellationToken);

            return _mapper.Map<CurrencyExchangeRateDto>(entity);
        }
    }
}
