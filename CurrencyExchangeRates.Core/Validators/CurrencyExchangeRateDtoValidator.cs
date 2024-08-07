using CurrencyExchangeRates.Models.DTO;
using FluentValidation;

namespace CurrencyExchangeRates.Core.Validators
{
    public class CurrencyExchangeRateDtoValidator : AbstractValidator<CurrencyExchangeRateDto?>
    {
        public CurrencyExchangeRateDtoValidator()
        {
            RuleFor(w => w)
                .NotNull()
                .WithMessage("Request cannot be null");

            RuleFor(w => w!.FromCurrencyCode)
                .NotEmpty()
                .WithMessage("FromCurrencyCode is required.");

            RuleFor(w => w!.ToCurrencyCode)
                .NotEmpty()
                .WithMessage("ToCurrencyCode is required.");

            RuleFor(w => w!.ToCurrencyCode)
                .NotEqual(w => w!.FromCurrencyCode)
                .WithMessage("ToCurrencyCode cannot be equal to FromCurrencyCode.");

            RuleFor(w => w!.ExchangeRate)
                .NotEqual(default(decimal))
                .WithMessage("ExchangeRate is required.");

            RuleFor(w => w!.AskPrice)
                .NotEqual(default(decimal))
                .WithMessage("AskPrice is required.");

            RuleFor(w => w!.BidPrice)
                .NotEqual(default(decimal))
                .WithMessage("BidPrice is required.");

            RuleFor(w => w!.LastRefreshed)
                .NotEqual(default(DateTime))
                .WithMessage("LastRefreshed is required.");
        }
    }
}
