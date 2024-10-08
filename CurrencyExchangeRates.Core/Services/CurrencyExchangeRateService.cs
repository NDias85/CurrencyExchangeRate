﻿using AutoMapper;
using CurrencyExchangeRates.Database.Repositories;
using CurrencyExchangeRates.Models.DTO;
using CurrencyExchangeRates.Models.Entities;
using CurrencyExchangeRates.Models.External;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace CurrencyExchangeRates.Core.Services
{
    public class CurrencyExchangeRateService : ICurrencyExchangeRateService
    {
        private readonly ILogger<CurrencyExchangeRateService> _logger;
        private readonly ICurrencyExchangeRateRepository _currencyExchangeRateRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IServiceBusQueueSender _serviceBusQueueSender;

        public CurrencyExchangeRateService(
            ILogger<CurrencyExchangeRateService> logger,
            ICurrencyExchangeRateRepository currencyExchangeRateRepository,
            IMapper mapper, HttpClient httpClient,
            IServiceBusQueueSender serviceBusQueueSender)
        {
            if (httpClient == null || httpClient.BaseAddress == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }

            _logger = logger;
            _currencyExchangeRateRepository = currencyExchangeRateRepository ?? throw new ArgumentNullException(nameof(currencyExchangeRateRepository));
            _mapper = mapper;
            _httpClient = httpClient;
            _serviceBusQueueSender = serviceBusQueueSender;
        }

        /// <inheritdoc/>
        public async Task<CurrencyExchangeRateDto> CreateCurrencyExchangeRateAsync(CurrencyExchangeRateDto request, CancellationToken cancellationToken)
        {
            var newEntity = _mapper.Map<CurrencyExchangeRate>(request);
            var result = await _currencyExchangeRateRepository.CreateCurrencyExchangeRateAsync(newEntity, cancellationToken);
                        
            var dto = _mapper.Map<CurrencyExchangeRateDto>(result);

            // Send event message
            await _serviceBusQueueSender.SendAsync(JsonSerializer.Serialize(dto));

            return dto;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteCurrencyExchangeRateAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken)
        {
            return await _currencyExchangeRateRepository.DeleteCurrencyExchangeRateAsync(currencyFrom, currencyTo, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<CurrencyExchangeRateDto?> GetCurrencyExchangeRateAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken)
        {
            var entity = await _currencyExchangeRateRepository.GetCurrencyExchangeRateAsync(currencyFrom, currencyTo, cancellationToken);
            if (entity != null)
            {
                return _mapper.Map<CurrencyExchangeRateDto>(entity);
            }

            // Get the result from the third party API (AlphaVantage)

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
            
            var dto = _mapper.Map<CurrencyExchangeRateDto>(newEntity);

            // Send event message
            await _serviceBusQueueSender.SendAsync(JsonSerializer.Serialize(dto));

            return dto;
        }

        /// <inheritdoc/>
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
