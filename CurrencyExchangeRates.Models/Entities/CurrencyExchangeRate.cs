using System.ComponentModel.DataAnnotations;

namespace CurrencyExchangeRates.Models.Entities
{
    [Microsoft.EntityFrameworkCore.Index(nameof(FromCurrencyCode), nameof(ToCurrencyCode), IsUnique = true)]
    public class CurrencyExchangeRate
    {
        public int Id { get; set; }

        [Required]
        public string? FromCurrencyCode { get; set; }

        [Required]
        public string? ToCurrencyCode { get; set; }

        public decimal ExchangeRate { get; set; }

        public DateTime LastRefreshed { get; set; }

        public decimal BidPrice { get; set; }

        public decimal AskPrice { get; set; }
    }
}
