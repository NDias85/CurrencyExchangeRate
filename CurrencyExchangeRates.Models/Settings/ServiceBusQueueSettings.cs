namespace CurrencyExchangeRates.Models.Settings
{
    public class ServiceBusQueueSettings
    {
        public required string QueueName { get; set; }
        public required string ConnectionString { get; set; }
    }
}
