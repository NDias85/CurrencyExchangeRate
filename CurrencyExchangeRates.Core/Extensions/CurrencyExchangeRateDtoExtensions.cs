using CurrencyExchangeRates.Core.Validators;
using CurrencyExchangeRates.Models.DTO;
using FluentValidation.Results;

namespace CurrencyExchangeRates.Core.Extensions
{
    public static class CurrencyExchangeRateDtoExtensions
    {
        public static ValidationResult GetValidationResults(this CurrencyExchangeRateDto? request)
        {
            var validator = new CurrencyExchangeRateDtoValidator();

            return validator.Validate(request);
        }
    }
}
