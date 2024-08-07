using CurrencyExchangeRates.Core.Extensions;
using CurrencyExchangeRates.Core.Services;
using CurrencyExchangeRates.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchangeRates.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyExchangeRateController : ControllerBase
    {
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService;

        public CurrencyExchangeRateController(ICurrencyExchangeRateService currencyExchangeRateService)
        {
            _currencyExchangeRateService = currencyExchangeRateService;
        }

        [HttpPost(Name = "CurrencyExchangeRate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CurrencyExchangeRateDto>> PostAsync(CurrencyExchangeRateDto request, CancellationToken cancellationToken)
        {
            var validationResults = request.GetValidationResults();
            if (!validationResults.IsValid)
            {
                return new BadRequestObjectResult(validationResults);
            }

            var result = await _currencyExchangeRateService.CreateCurrencyExchangeRateAsync(request, cancellationToken);
            return result == null ? BadRequest() : result;
        }

        [HttpGet(Name = "CurrencyExchangeRate/{currencyFrom}/{currencyTo}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CurrencyExchangeRateDto>> GetAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken)
        {
            var result = await _currencyExchangeRateService.GetCurrencyExchangeRateAsync(currencyFrom, currencyTo, cancellationToken);
            return result == null ? NotFound() : result;
        }

        [HttpPut(Name = "CurrencyExchangeRate/{FromCurrencyCode}/{ToCurrencyCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CurrencyExchangeRateDto>> PutAsync(CurrencyExchangeRateDto request, CancellationToken cancellationToken)
        {
            var validationResults = request.GetValidationResults();
            if (!validationResults.IsValid)
            {
                return new BadRequestObjectResult(validationResults);
            }

            var result = await _currencyExchangeRateService.UpdateCurrencyExchangeRateAsync(request, cancellationToken);
            return result == null ? NotFound() : result;
        }

        [HttpDelete(Name = "CurrencyExchangeRate/{currencyFrom}/{currencyTo}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<StatusCodeResult> DeleteAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken)
        {
            var result = await _currencyExchangeRateService.DeleteCurrencyExchangeRateAsync(currencyFrom, currencyTo, cancellationToken);
            return !result ? NotFound() : NoContent();
        }
    }
}
