using System.Text.Json.Serialization;

namespace CurrencyExchangeRates.Models.External
{
    public class BaseResponse
    {
        [JsonPropertyName("Realtime Currency Exchange Rate")]
        public RealTimeCurrencyExchangeRate? RealTimeCurrencyExchangeRate { get; set; }
    }
}
