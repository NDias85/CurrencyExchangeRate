using CurrencyExchangeRates.Core.Extensions;
using CurrencyExchangeRates.Core.Services;
using CurrencyExchangeRates.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace CurrencyExchangeRates.FunctionApp
{
    public class CurrencyExchangeRateFunction
    {
        private readonly ICurrencyExchangeRateService _currencyExchangeRateService;

        public CurrencyExchangeRateFunction(ICurrencyExchangeRateService currencyExchangeRateService)
        {
            _currencyExchangeRateService = currencyExchangeRateService ?? throw new ArgumentNullException(nameof(currencyExchangeRateService));
        }

        [FunctionName("CurrencyExchangeRateGet")]
        public async Task<IActionResult> RunGet(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "CurrencyExchangeRateFunction/{currencyFrom}/{currencyTo}")] 
            HttpRequest req,
            string currencyFrom,
            string currencyTo,
            CancellationToken cancellationToken)
        {
            var result = await _currencyExchangeRateService.GetCurrencyExchangeRateAsync(currencyFrom, currencyTo, cancellationToken);
            return result == null ? new NotFoundResult() : new OkObjectResult(result);
        }

        [FunctionName("CurrencyExchangeRatePost")]
        public async Task<IActionResult> RunPost(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "CurrencyExchangeRateFunction")]
            HttpRequest req,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
                var request = JsonConvert.DeserializeObject<CurrencyExchangeRateDto>(requestBody);

                var validationResults = request.GetValidationResults();
                if (!validationResults.IsValid)
                {
                    return new BadRequestObjectResult(validationResults.Errors.Select(s => s.ErrorMessage));
                }

                var result = await _currencyExchangeRateService.CreateCurrencyExchangeRateAsync(request!, cancellationToken);
                return result == null ? new BadRequestResult() : new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to create a new CurrencyExchangeRate");
                return new BadRequestResult();
            }
        }

        [FunctionName("CurrencyExchangeRatePut")]
        public async Task<IActionResult> RunPut(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "CurrencyExchangeRateFunction/{currencyFrom}/{currencyTo}")]
            HttpRequest req,
            string currencyFrom,
            string currencyTo,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            try
            {
                var requestBody = await new StreamReader(req.Body).ReadToEndAsync(cancellationToken);
                var request = JsonConvert.DeserializeObject<CurrencyExchangeRateDto>(requestBody);
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
                return result == null ? new BadRequestResult() : new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to create a new CurrencyExchangeRate");
                return new BadRequestResult();
            }
        }

        [FunctionName("CurrencyExchangeRateDelete")]
        public async Task<StatusCodeResult> RunDelete(
           [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "CurrencyExchangeRateFunction/{currencyFrom}/{currencyTo}")]
           HttpRequest req,
           string currencyFrom,
           string currencyTo,
           CancellationToken cancellationToken)
        {
            var result = await _currencyExchangeRateService.DeleteCurrencyExchangeRateAsync(currencyFrom, currencyTo, cancellationToken);
            return !result ? new NotFoundResult() : new NoContentResult();
        }
    }
}
