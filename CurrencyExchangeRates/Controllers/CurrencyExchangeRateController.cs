using CurrencyExchangeRates.Core.Extensions;
using CurrencyExchangeRates.Core.Services;
using CurrencyExchangeRates.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchangeRates.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyExchangeRateController : ControllerBase
    {
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService;

        public CurrencyExchangeRateController(ICurrencyExchangeRateService currencyExchangeRateService)
        {
            _currencyExchangeRateService = currencyExchangeRateService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CurrencyExchangeRateDto>> PostAsync(CurrencyExchangeRateDto request, CancellationToken cancellationToken)
        {
            var validationResults = request.GetValidationResults();
            if (!validationResults.IsValid)
            {
                return new BadRequestObjectResult(validationResults.Errors.Select(s => s.ErrorMessage));
            }

            var result = await _currencyExchangeRateService.CreateCurrencyExchangeRateAsync(request, cancellationToken);
            return result == null ? BadRequest() : result;
        }

        [HttpGet]
        [Route("{currencyFrom}/{currencyTo}", Name = "GetAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CurrencyExchangeRateDto>> GetAsync(string currencyFrom, string currencyTo, CancellationToken cancellationToken)
        {
            var result = await _currencyExchangeRateService.GetCurrencyExchangeRateAsync(currencyFrom, currencyTo, cancellationToken);
            return result == null ? NotFound() : result;
        }

        [HttpPut]
        [Route("{currencyFrom}/{currencyTo}", Name = "PutAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CurrencyExchangeRateDto>> PutAsync(string currencyFrom, string currencyTo, CurrencyExchangeRateDto request, CancellationToken cancellationToken)
        {
            if (request is not null)
            {
                request.FromCurrencyCode = currencyFrom;
                request.ToCurrencyCode = currencyTo;
            }

            var validationResults = request.GetValidationResults();
            if (!validationResults.IsValid)
            {
                return new BadRequestObjectResult(validationResults.Errors.Select(s => s.ErrorMessage));
            }

            var result = await _currencyExchangeRateService.UpdateCurrencyExchangeRateAsync(request!, cancellationToken);
            return result == null ? NotFound() : result;
        }

        [HttpDelete]
        [Route("{currencyFrom}/{currencyTo}", Name = "DeleteAsync")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<StatusCodeResult> DeleteAsync([FromRoute] string currencyFrom, [FromRoute] string currencyTo, CancellationToken cancellationToken)
        {
            var result = await _currencyExchangeRateService.DeleteCurrencyExchangeRateAsync(currencyFrom, currencyTo, cancellationToken);
            return !result ? NotFound() : NoContent();
        }
    }
}
