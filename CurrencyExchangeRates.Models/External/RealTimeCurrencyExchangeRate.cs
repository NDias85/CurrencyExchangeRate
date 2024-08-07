using System.Text.Json.Serialization;

namespace CurrencyExchangeRates.Models.External
{
    public class RealTimeCurrencyExchangeRate
    {
        [JsonPropertyName("1. From_Currency Code")]
        public string? FromCurrencyCode { get; set; }

        [JsonPropertyName("3. To_Currency Code")]
        public string? ToCurrencyCode { get; set; }

        [JsonPropertyName("5. Exchange Rate")]
        public decimal ExchangeRate { get; set; }

        [JsonPropertyName("6. Last Refreshed")]
        public string? LastRefreshed { get; set; }

        [JsonPropertyName("8. Bid Price")]
        public decimal BidPrice { get; set; }

        [JsonPropertyName("9. Ask Price")]
        public decimal AskPrice { get; set; }
    }
}
