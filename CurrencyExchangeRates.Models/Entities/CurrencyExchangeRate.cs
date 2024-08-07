using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column(TypeName = "decimal(18, 6)")]
        public decimal ExchangeRate { get; set; }

        public DateTime LastRefreshed { get; set; }

        [Column(TypeName = "decimal(18, 6)")]
        public decimal BidPrice { get; set; }

        [Column(TypeName = "decimal(18, 6)")]
        public decimal AskPrice { get; set; }
    }
}
