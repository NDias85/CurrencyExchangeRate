namespace CurrencyExchangeRates.Models.DTO
{
    public class CurrencyExchangeRateDto
    {
        public string? FromCurrencyCode { get; set; }

        public string? ToCurrencyCode { get; set; }

        public decimal ExchangeRate { get; set; }

        public DateTime LastRefreshed { get; set; }

        public decimal BidPrice { get; set; }

        public decimal AskPrice { get; set; }
    }
}
